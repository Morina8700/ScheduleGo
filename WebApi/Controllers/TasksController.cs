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
                CreatedAt = task.CreatedAt,

                EventId = task.EventId,
                EventTitle = task.Event != null ? task.Event.Title : null,
                EventStart = task.Event != null ? task.Event.Start : null
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoItemDto>> GetTask(Guid id)
    {
        var task = await context.ToDoItems
            .AsNoTracking()
            .Include(task => task.Event)
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
        Event? linkedEvent = null;

        if (request.EventId.HasValue)
        {
            linkedEvent = await context.Events.FindAsync(request.EventId.Value);

            if (linkedEvent is null)
            {
                return BadRequest("The selected event does not exist.");
            }
        }

        var task = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description,
            DueAt = request.DueAt,
            EventId = request.EventId,
            Event = linkedEvent,
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
        Event? linkedEvent = null;

        if (request.EventId.HasValue)
        {
            linkedEvent = await context.Events.FindAsync(request.EventId.Value);

            if (linkedEvent is null)
            {
                return BadRequest("The selected event does not exist.");
            }
        }
        var task = await context.ToDoItems.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueAt = request.DueAt;
        task.Priority = request.Priority;
        task.EventId = request.EventId;
        task.Event = linkedEvent;

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
            CreatedAt = task.CreatedAt,

            EventId = task.EventId,
            EventTitle = task.Event?.Title,
            EventStart = task.Event?.Start
        };
    }
}