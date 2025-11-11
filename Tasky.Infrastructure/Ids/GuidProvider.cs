using Tasky.Application.Interfaces;
namespace Tasky.Infrastructure.Ids;
public sealed class GuidProvider : IGuidProvider
{
    public Guid NewGuid() => Guid.NewGuid();
}
