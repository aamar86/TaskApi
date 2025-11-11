using Tasky.Application.Interfaces;
namespace Tasky.Infrastructure.Time;
public sealed class SystemClock : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
