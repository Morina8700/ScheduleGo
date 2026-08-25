using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models;

public class SaveTodoItemRequest
{
    [Required(ErrorMessage = "A title is required.")]
    [MaxLength(150)]
    public string Title { get; set; } = "";

    [MaxLength(1000)]
    public string Description { get; set; } = "";

    public DateTime? DueAt { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Normal;
    public Guid? EventId { get; set; }
}