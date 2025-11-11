using Tasky.Application.Common;
using Tasky.Application.DTOs;
using Tasky.Application.Interfaces;
using Tasky.Domain.Entities;

namespace Tasky.Application.Services;

public sealed class TaskService(ITaskRepository repo, IDateTimeProvider clock, IGuidProvider guid) : ITaskService
{
    public Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct) => repo.GetAllAsync(ct);
    public Task<TaskItem?> GetAsync(Guid id, CancellationToken ct) => repo.GetByIdAsync(id, ct);

    public async Task<TaskItem> CreateAsync(CreateTaskRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
            throw new ProblemException("Title is required.", 400);
        if (req.DueAt is not null && req.DueAt < clock.Now)
            throw new ProblemException("DueAt cannot be in the past.", 400);

        var entity = new TaskItem(guid.NewGuid(), req.Title.Trim(), clock.Now, req.DueAt, false);
        await repo.AddAsync(entity, ct);
        await repo.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<TaskItem?> UpdateAsync(Guid id, UpdateTaskRequest req, CancellationToken ct)
    {
        var existing = await repo.GetByIdAsync(id, ct);
        if (existing is null) return null;

        var title = req.Title is null ? existing.Title :
            string.IsNullOrWhiteSpace(req.Title) ? throw new ProblemException("Title cannot be empty.", 400) :
            req.Title.Trim();

        var dueAt = req.DueAt ?? existing.DueAt;
        if (dueAt is not null && dueAt < clock.Now)
            throw new ProblemException("DueAt cannot be in the past.", 400);

        var updated = existing with { Title = title, DueAt = dueAt, IsCompleted = req.IsCompleted ?? existing.IsCompleted };
        repo.Update(updated);
        await repo.SaveChangesAsync(ct);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var existing = await repo.GetByIdAsync(id, ct);
        if (existing is null) return false;
        repo.Delete(existing);
        await repo.SaveChangesAsync(ct);
        return true;
    }
}
