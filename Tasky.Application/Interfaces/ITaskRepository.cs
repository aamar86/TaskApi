using Tasky.Domain.Entities;

namespace Tasky.Application.Interfaces;

public interface ITaskRepository : IBaseRepository<TaskItem>
{
    Task<IReadOnlyList<TaskItem>> GetDueWithinAsync(TimeSpan window, CancellationToken ct);
}
