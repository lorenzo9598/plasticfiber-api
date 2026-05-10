# Domain model (canonical)

This file is the **source of truth** for domain types and API-facing DTOs in this repository. C# types under `Plasticfiber.Api/Models/` must match it. When you add or change a model in code, update this document in the same change.

## Conventions

- **Naming in tables**: C# type names use `PascalCase`. JSON payloads use `camelCase` unless otherwise noted (ASP.NET Core default serializer).
- **Records**: Prefer `record` types for immutable DTOs.

## Types

### `TestHealthResponse`

Alive-check payload returned by `GET /api/test`.

| Field / JSON | C# property | Type   | Required | Description                          |
|--------------|-------------|--------|----------|--------------------------------------|
| `status`     | `Status`    | string | yes      | Service status (e.g. `"running"`).   |
| `message`    | `Message`   | string | yes      | Human-readable message for operators. |

**Example JSON**

```json
{
  "status": "running",
  "message": "hello world"
}
```
