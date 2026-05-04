namespace task_generator.Dto
{
    public class GitFileDto
    {
        public string FilePath { get; set; } = "";
        public string Status { get; set; } = "";
    }

    public class GitFileContentDto
    {
        public string FilePath { get; set; } = "";
        public string Content { get; set; } = "";
    }

}
