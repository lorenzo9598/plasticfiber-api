using Plasticfiber.Api.Interfaces;
using Plasticfiber.Api.Models;

namespace Plasticfiber.Api.Services;

public sealed class TestService : ITestService
{
    public Task<TestHealthResponse> GetTestHealthAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new TestHealthResponse("running", "hello world"));
    }
}
