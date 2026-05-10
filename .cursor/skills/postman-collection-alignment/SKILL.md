---
name: postman-collection-alignment
description: Compares docs/postman/PlasticFiber-API.postman_collection.json with docs/API-SPEC.md and implemented HTTP routes, reports mismatches, and proposes edits so the Postman collection stays import-ready and aligned. Use when auditing endpoints, before merge, or after API-SPEC or route changes.
disable-model-invocation: true
---

# Postman collection alignment

## Instructions

1. Read `docs/API-SPEC.md` and treat it as the **canonical** list of public HTTP endpoints (method, path, auth if any, request/response expectations).
2. Inventory implemented routes from `Plasticfiber.Api/Program.cs` and any `*Endpoints.cs` files or controllers (same sources as **api-spec-alignment**).
3. Read `docs/postman/PlasticFiber-API.postman_collection.json` and extract each request’s HTTP method and resolved path (respect collection variables; default `baseUrl` should match `Plasticfiber.Api/Properties/launchSettings.json` for the primary local profile unless the spec documents otherwise).
4. Compare:
   - Every spec endpoint should have a corresponding Postman request (matching verb + path relative to `{{baseUrl}}`).
   - Every Postman request should map to a spec entry and to code (or flag as intentional playground-only — prefer removing stray requests).
   - Collection variable **`baseUrl`**: default value must stay aligned with the app’s documented local URL (e.g. `http` profile in `launchSettings.json`) when that is how developers run the API.
5. Produce a short report:
   - **Match**: collection covers spec endpoints with correct methods/paths and sensible folder names.
   - **Drift**: missing requests, extra requests, wrong method/path, stale `baseUrl`, missing auth headers if the API requires auth.
   - **Recommendation**: edit **`docs/postman/PlasticFiber-API.postman_collection.json`** (and/or spec/code per **api-spec-alignment**) so the collection, spec, and implementation stay consistent.

## Examples

- Spec adds `POST /api/widgets`; collection still only has `GET /api/test` → add a folder/request for the new endpoint with appropriate body and headers.
- Collection contains `GET /api/legacy` not in spec or code → remove it or document the route in `docs/API-SPEC.md` and implement it.
- `launchSettings.json` changes the `http` profile port; collection `baseUrl` still uses the old port → update the default `baseUrl` in the collection variables.
