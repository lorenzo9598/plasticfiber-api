namespace Plasticfiber.Api.Models;

/// <summary>
/// Health probe payload for <c>GET /api/test</c>. Documented in <c>docs/DOMAIN-MODEL.md</c>.
/// </summary>
public sealed record TestHealthResponse(string Status, string Message);
