namespace WebApi.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public DateTime? DueAt { get; set; }
        public bool IsCompleted { get; set; }

        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? EventId { get; set; }

        public Event? Event { get; set; }
    }

    public enum TaskPriority
    {
        Low,
        Normal,
        High

    }
}
