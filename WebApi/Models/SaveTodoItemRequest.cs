using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class SaveTodoItemRequest
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = "";

    [MaxLength(1000)]
    public string Description { get; set; } = "";

    public DateTime? DueAt { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Normal;
    public Guid? EventId { get; set; }
}