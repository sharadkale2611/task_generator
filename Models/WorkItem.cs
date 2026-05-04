using System.Text.Json.Serialization;

namespace task_generator.Models
{
    public class WorkItem
    {
        public int WorkItemId { get; set; }

        public int ProjectId { get; set; }
        public int? SprintId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Type { get; set; } = "Task";
        // Epic | Story | Task | Bug

        public string Status { get; set; } = "ToDo";

        public int? ParentWorkItemId { get; set; } // 🔥 hierarchy
        public int Order { get; set; } // 🔥 ADD THIS


        // 🔥 JIRA MAPPING
        public string? ExternalId { get; set; }   // JIRA-123
        public string? ExternalSystem { get; set; } // Jira

        public int Points { get; set; }

        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        public int TenantId { get; set; } // 🔥 VERY IMPORTANT

        // Navigation
        [JsonIgnore]
        public WorkItem? Parent { get; set; }
        public ICollection<WorkItem> Children { get; set; } = new List<WorkItem>();
    }
}
