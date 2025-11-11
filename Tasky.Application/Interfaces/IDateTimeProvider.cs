namespace Tasky.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
