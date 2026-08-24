using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController(
    ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TodoItemDto>>> GetTasks()
    {
        var tasks = await context.ToDoItems
            .AsNoTracking()
            .OrderBy(task => task.IsCompleted)
            .ThenBy(task => task.DueAt)
            .ThenByDescending(task => task.CreatedAt)
            .Select(task => new TodoItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueAt = task.DueAt,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoItemDto>> GetTask(Guid id)
    {
        var task = await context.ToDoItems
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task is null)
        {
            return NotFound();
        }

        return Ok(ToDto(task));
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> CreateTask(
        SaveTodoItemRequest request)
    {
        var task = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description,
            DueAt = request.DueAt,
            Priority = request.Priority,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        context.ToDoItems.Add(task);
        await context.SaveChangesAsync();

        var dto = ToDto(task);

        return CreatedAtAction(
            nameof(GetTask),
            new { id = task.Id },
            dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TodoItemDto>> UpdateTask(
        Guid id,
        SaveTodoItemRequest request)
    {
        var task = await context.ToDoItems.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueAt = request.DueAt;
        task.Priority = request.Priority;

        await context.SaveChangesAsync();

        return Ok(ToDto(task));
    }

    [HttpPatch("{id:guid}/completed")]
    public async Task<ActionResult<TodoItemDto>> SetCompleted(
        Guid id,
        SetTodoItemCompletedRequest request)
    {
        var task = await context.ToDoItems.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        task.IsCompleted = request.IsCompleted;

        await context.SaveChangesAsync();

        return Ok(ToDto(task));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await context.ToDoItems.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        context.ToDoItems.Remove(task);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private static TodoItemDto ToDto(ToDoItem task)
    {
        return new TodoItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueAt = task.DueAt,
            IsCompleted = task.IsCompleted,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt
        };
    }
}