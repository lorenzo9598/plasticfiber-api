---
name: api-spec-alignment
description: Compares implemented HTTP routes in the Plasticfiber API with docs/API-SPEC.md, reports mismatches, and proposes updates to Minimal API mappings or the canonical spec. Use when auditing endpoints, before merge, or when the user asks to sync routes with API-SPEC.md.
disable-model-invocation: true
---

# API spec alignment

## Instructions

1. Read `docs/API-SPEC.md` and treat it as the **canonical** list of HTTP endpoints (method, path, purpose, status codes, request/response bodies).
2. Inventory routes from `Plasticfiber.Api/Program.cs` and any future `*Endpoints.cs` or controllers.
3. For each spec entry, confirm a matching implementation (verb + path + content type/shape). For each `Map*` route in code, confirm it appears in the spec or flag it as undocumented.
4. Compare response/request DTOs to the spec and to `docs/DOMAIN-MODEL.md` where the spec links types.
5. Produce a short report:
   - **Match**: routes and contracts aligned.
   - **Drift**: spec-only routes, code-only routes, wrong verb/path, wrong documented status or body.
   - **Recommendation**: for each item, change **`docs/API-SPEC.md`** or **code** (and domain doc if DTOs change) so all three stay consistent. When routes or the spec change, also update **`docs/postman/PlasticFiber-API.postman_collection.json`** in the same change; use `.cursor/skills/postman-collection-alignment/SKILL.md` to verify the collection matches spec and code.

## Examples

- Spec documents `GET /api/test` returning `TestHealthResponse`; code maps `/api/test` but returns an anonymous object with different property names → recommend fixing code to use `TestHealthResponse` or updating the spec if intentional.
- Code adds `GET /api/health` not in spec → recommend adding a section to `docs/API-SPEC.md` or removing the route.
