namespace Tasky.Application.DTOs;
public sealed record CreateTaskRequest(string Title, DateTimeOffset? DueAt);
