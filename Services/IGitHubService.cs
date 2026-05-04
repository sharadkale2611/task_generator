using System.Text;
using System.Text.Json;
using task_generator.Dto;

namespace task_generator.Services
{
    public interface IGitHubService
    {
        Task<List<GitFileDto>> GetChangedFiles(string repoUrl, string baseBranch, string branch);
        Task<List<GitFileContentDto>> GetFileContents(string repoUrl, string branch, List<GitFileDto> files);
    }

    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public GitHubService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        // ================================
        // 🔹 GET CHANGED FILES
        // ================================
        public async Task<List<GitFileDto>> GetChangedFiles(string repoUrl, string baseBranch, string branch)
        {
            var (owner, repo) = ParseRepo(repoUrl);

            var url = $"https://api.github.com/repos/{owner}/{repo}/compare/{baseBranch}...{branch}";

            var request = CreateRequest(url);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"GitHub compare API failed: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("files", out var files))
                return new List<GitFileDto>();

            return files.EnumerateArray()
                .Select(f => new GitFileDto
                {
                    FilePath = f.GetProperty("filename").GetString()!,
                    Status = f.GetProperty("status").GetString()!
                })
                .ToList();
        }

        // ================================
        // 🔹 GET FILE CONTENTS
        // ================================
        public async Task<List<GitFileContentDto>> GetFileContents(
            string repoUrl,
            string branch,
            List<GitFileDto> files)
        {
            var (owner, repo) = ParseRepo(repoUrl);

            var allowedExtensions = new[] { ".cs", ".ts", ".js" }; // remove json

            var selectedFiles = files
                .Where(f => allowedExtensions.Any(ext =>
                    f.FilePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                .Take(5) // 🔥 limit for AI performance
                .ToList();

            var result = new List<GitFileContentDto>();

            foreach (var file in selectedFiles)
            {
                try
                {
                    var url = $"https://api.github.com/repos/{owner}/{repo}/contents/{file.FilePath}?ref={branch}";

                    var request = CreateRequest(url);

                    var response = await _http.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                        continue;

                    var json = await response.Content.ReadAsStringAsync();

                    var doc = JsonDocument.Parse(json);

                    if (!doc.RootElement.TryGetProperty("content", out var contentElement))
                        continue;

                    var contentEncoded = contentElement.GetString();

                    if (string.IsNullOrEmpty(contentEncoded))
                        continue;

                    var content = Encoding.UTF8.GetString(
                        Convert.FromBase64String(contentEncoded.Replace("\n", ""))
                    );

                    result.Add(new GitFileContentDto
                    {
                        FilePath = file.FilePath,
                        Content = content.Length > 2000
                            ? content.Substring(0, 2000)
                            : content
                    });
                }
                catch
                {
                    // skip problematic file
                    continue;
                }
            }

            return result;
        }

        // ================================
        // 🔹 COMMON REQUEST BUILDER
        // ================================
        private HttpRequestMessage CreateRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("User-Agent", "task-generator");

            var token = _config["GitHub:Token"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            return request;
        }

        // ================================
        // 🔹 SAFE REPO PARSER
        // ================================
        private (string owner, string repo) ParseRepo(string repoUrl)
        {
            if (string.IsNullOrWhiteSpace(repoUrl))
                throw new Exception("Invalid repository URL");

            var cleaned = repoUrl
                .Replace("https://github.com/", "")
                .Replace(".git", "")
                .TrimEnd('/');

            var parts = cleaned.Split('/');

            if (parts.Length < 2)
                throw new Exception("Invalid GitHub repo URL format");

            return (parts[0], parts[1]);
        }
    }
}