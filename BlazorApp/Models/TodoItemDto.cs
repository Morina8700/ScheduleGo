namespace BlazorApp.Models
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? DueAt { get; set; }
        public bool IsCompleted { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? EventId { get; set; }
        public string? EventTitle { get; set; }
        public DateTime? EventStart { get; set; }
    }

}
