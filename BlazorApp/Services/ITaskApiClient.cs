using BlazorApp.Models;

namespace BlazorApp.Services
{
        public interface ITaskApiClient
        {
            Task<IReadOnlyList<TodoItemDto>> GetAllAsync();
            Task<TodoItemDto> GetAsync(Guid id);
            Task<TodoItemDto> CreateAsync(SaveTodoItemRequest request);
            Task<TodoItemDto> UpdateAsync(Guid id, SaveTodoItemRequest request);
            Task DeleteAsync(Guid id);
            Task SetCompletedAsync(Guid id, bool completed);
        }
    
}
