---
name: domain-model-alignment
description: Compares C# domain and DTO types in the Plasticfiber API repo with docs/DOMAIN-MODEL.md, reports mismatches, and proposes updates to code or the canonical markdown. Use when auditing models, before merge, or when the user asks to sync DTOs with DOMAIN-MODEL.md.
disable-model-invocation: true
---

# Domain model alignment

## Instructions

1. Read `docs/DOMAIN-MODEL.md` and treat it as the **canonical** contract for documented types (fields, types, required vs optional, JSON names).
2. Find matching C# types (records/classes) under `Plasticfiber.Api/` — especially `Models/` and any future domain folders.
3. For each documented type, compare:
   - Type name and namespace (repo convention: DTOs in `Plasticfiber.Api.Models` unless doc says otherwise).
   - Properties: name (PascalCase in C# vs `camelCase` JSON), CLR type, nullability, and required semantics.
   - Any type in code that is a public API contract but **missing** from the doc → flag and suggest a doc addition (or removal from public surface).
4. Produce a short report:
   - **Match**: types and fields aligned.
   - **Drift**: list each discrepancy (doc-only, code-only, wrong type/name, wrong nullability).
   - **Recommendation**: for each item, state whether to change **code** or **`docs/DOMAIN-MODEL.md`** so they agree (prefer updating both in one change when adding features).

## Examples

- Doc lists `message` as required string; C# has `Message` but type is `string?` → recommend tightening to `string` or documenting optional in the MD table.
- Code adds `TestHealthResponse` field `buildId` not in doc → recommend extending `docs/DOMAIN-MODEL.md` and the example JSON block.
