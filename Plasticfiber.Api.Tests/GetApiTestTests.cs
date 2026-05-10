using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Plasticfiber.Api.Models;
using Xunit;

namespace Plasticfiber.Api.Tests;

public class GetApiTestTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetApiTestTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_api_test_returns_200_and_TestHealthResponse_json()
    {
        var response = await _client.GetAsync("/api/test");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<TestHealthResponse>();
        Assert.NotNull(body);
        Assert.Equal("running", body.Status);
        Assert.Equal("hello world", body.Message);
    }
}
