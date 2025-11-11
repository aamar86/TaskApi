namespace Tasky.Domain.Entities;

public sealed record TaskItem(
    Guid Id,
    string Title,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DueAt,
    bool IsCompleted
);
