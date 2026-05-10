# HTTP API specification (canonical)

This file is the **source of truth** for public HTTP endpoints. Implementations (controllers in `Plasticfiber.Api/Controllers/`, or Minimal APIs when used) must match it. When routes or contracts change, update this document in the same change, and update or add integration tests in `Plasticfiber.Api.Tests` when response shape or behavior changes.

**Live OpenAPI (Development):** With `dotnet run` in the Development environment, Swashbuckle exposes the interactive UI at `/swagger` and the OpenAPI document at `/swagger/v1/swagger.json` (use your local base URL, for example `https://localhost:7xxx/swagger`).

## Conventions

- Paths are relative to the host root unless a base path is documented.
- Request/response bodies use JSON with `application/json` unless stated otherwise.

## Endpoints

### `GET /api/test`

**Purpose:** Liveness probe; proves the API process is running.

**Response**

- **200** `application/json` — body matches [`TestHealthResponse`](./DOMAIN-MODEL.md#testhealthresponse).

**Example**

```http
GET /api/test HTTP/1.1
```

```json
{
  "status": "running",
  "message": "hello world"
}
```
