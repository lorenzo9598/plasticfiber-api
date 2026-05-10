using Microsoft.AspNetCore.Mvc;
using Plasticfiber.Api.Interfaces;
using Plasticfiber.Api.Models;

namespace Plasticfiber.Api.Controllers;

[ApiController]
[Route("api/test")]
public sealed class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TestHealthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TestHealthResponse>> GetAsync(CancellationToken cancellationToken)
    {
        var body = await _testService.GetTestHealthAsync(cancellationToken);
        return Ok(body);
    }
}
