using BlazorApp.Models;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
        public sealed class TaskApiClient(HttpClient http) : ITaskApiClient
        {
            public async Task<IReadOnlyList<TodoItemDto>> GetAllAsync()
            {
                return await http.GetFromJsonAsync<List<TodoItemDto>>(
                    "api/tasks") ?? [];
            }

            public async Task<TodoItemDto> CreateAsync(
                SaveTodoItemRequest request)
            {
                var response = await http.PostAsJsonAsync(
                    "api/tasks",
                    request);

                response.EnsureSuccessStatusCode();

                return await response.Content
                    .ReadFromJsonAsync<TodoItemDto>()
                    ?? throw new InvalidOperationException(
                        "The API returned an empty response.");
            }

            public async Task<TodoItemDto> UpdateAsync(
                Guid id,
                SaveTodoItemRequest request)
            {
                var response = await http.PutAsJsonAsync(
                    $"api/tasks/{id}",
                    request);

                response.EnsureSuccessStatusCode();

                return await response.Content
                    .ReadFromJsonAsync<TodoItemDto>()
                    ?? throw new InvalidOperationException(
                        "The API returned an empty response.");
            }

            public async Task DeleteAsync(Guid id)
            {
                var response = await http.DeleteAsync($"api/tasks/{id}");
                response.EnsureSuccessStatusCode();
            }

            public async Task SetCompletedAsync(Guid id, bool completed)
            {
                var response = await http.PatchAsJsonAsync(
                    $"api/tasks/{id}/completed",
                    new { IsCompleted = completed });

                response.EnsureSuccessStatusCode();
            }

        public async Task<TodoItemDto> GetAsync(Guid id)
        {
            var response = await http.GetAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content
                   .ReadFromJsonAsync<TodoItemDto>()
                   ?? throw new InvalidOperationException(
                       "The API returned an empty response.");
        }
        }
    }
