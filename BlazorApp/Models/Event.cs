namespace BlazorApp.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
