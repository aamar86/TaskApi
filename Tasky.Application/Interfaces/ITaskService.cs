using Tasky.Domain.Entities;
using Tasky.Application.DTOs;

namespace Tasky.Application.Interfaces;

public interface ITaskService
{
    Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct);
    Task<TaskItem?> GetAsync(Guid id, CancellationToken ct);
    Task<TaskItem> CreateAsync(CreateTaskRequest req, CancellationToken ct);
    Task<TaskItem?> UpdateAsync(Guid id, UpdateTaskRequest req, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
