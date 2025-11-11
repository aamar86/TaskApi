namespace Tasky.Application.DTOs;
public sealed record UpdateTaskRequest(string? Title, DateTimeOffset? DueAt, bool? IsCompleted);
