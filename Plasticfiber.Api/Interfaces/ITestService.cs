using Plasticfiber.Api.Models;

namespace Plasticfiber.Api.Interfaces;

/// <summary>
/// Liveness payload for <c>GET /api/test</c>; will gain persistence later.
/// </summary>
public interface ITestService
{
    Task<TestHealthResponse> GetTestHealthAsync(CancellationToken cancellationToken = default);
}
