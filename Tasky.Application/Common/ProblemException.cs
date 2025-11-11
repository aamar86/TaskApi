namespace Tasky.Application.Common;

public sealed class ProblemException(string message, int statusCode = 400) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
