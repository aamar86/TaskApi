using Microsoft.EntityFrameworkCore;
using Tasky.Application.Interfaces;
using Tasky.Domain.Entities;
using Tasky.Infrastructure.Persistence;

namespace Tasky.Infrastructure.Repositories;

public class EfTaskRepository : BaseRepository<TaskItem>, ITaskRepository
{
    public EfTaskRepository(TaskyDbContext context) : base(context) { }

    public async Task<IReadOnlyList<TaskItem>> GetDueWithinAsync(TimeSpan window, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var upper = now + window;
        return await _dbSet
            .Where(t => !t.IsCompleted && t.DueAt <= upper)
            .OrderBy(t => t.DueAt)
            .ToListAsync(ct);
    }
}
