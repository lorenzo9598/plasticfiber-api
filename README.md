# plasticfiber-api

Web API in **.NET 8** (ASP.NET Core) che fa da scheletro per il backend di Plasticfiber. Espone al momento un solo endpoint di health/test (`GET /api/test`) usato come liveness probe e come banco di prova per la pipeline (DI, Swagger, test di integrazione).

Il codice segue una struttura a layer con cartelle e namespace allineati sotto `Plasticfiber.Api.*`:

- `Plasticfiber.Api/Models/` — DTO immutabili (`record`) esposti via JSON.
- `Plasticfiber.Api/Interfaces/` — contratti dei servizi applicativi.
- `Plasticfiber.Api/Services/` — implementazioni dei servizi, registrate in DI.
- `Plasticfiber.Api/Controllers/` — controller `[ApiController]` attribute-routed.
- `Plasticfiber.Api/Program.cs` — composition root (DI, Swagger in `Development`, `MapControllers`).
- `Plasticfiber.Api.Tests/` — test xUnit basati su `WebApplicationFactory<Program>`.

## Come eseguire l'API

Prerequisiti: **.NET SDK 8**.

Dalla root della solution:

```bash
dotnet run --project Plasticfiber.Api
```

Le URL di ascolto sono definite in [`Plasticfiber.Api/Properties/launchSettings.json`](Plasticfiber.Api/Properties/launchSettings.json):

- profilo `http`: `http://localhost:5165`
- profilo `https`: `https://localhost:7195` (con fallback HTTP su `http://localhost:5165`)
- profilo `http-release` (test locale stile produzione, configurazione **Release**): `https://localhost:7080` e `http://localhost:5080`, ambiente `Production` (nessuna Swagger UI)

La porta **5000** non è usata dalle configurazioni di questo repo, così resta libera per altri tool di debug.

Per scegliere esplicitamente un profilo:

```bash
dotnet run --project Plasticfiber.Api --launch-profile https
```

Per un run in configurazione Release con le porte dedicate al profilo `http-release`:

```bash
dotnet run -c Release --project Plasticfiber.Api --launch-profile http-release
```

Verifica rapida che il processo sia in piedi:

```bash
curl http://localhost:5165/api/test
```

Per la suite di test:

```bash
dotnet test
```

## Build e rilascio (Release)

Compilazione della solution in Release:

```bash
dotnet build PlasticfiberApi.sln -c Release
```

Pubblicazione dell’API in una cartella (esempio `./publish` alla root del repo):

```bash
dotnet publish Plasticfiber.Api -c Release -o ./publish
```

**Eseguire l’output pubblicato** (dalla cartella `publish`), impostando esplicitamente le URL di ascolto (consigliato: stesse porte HTTP del profilo `http-release`, senza usare la porta 5000):

```bash
cd publish
ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://localhost:5080 dotnet Plasticfiber.Api.dll
```

In alternativa, per provare un’esecuzione Release **senza** passare da `publish`, si possono usare le URL del profilo `http-release` tramite:

```bash
dotnet run -c Release --project Plasticfiber.Api --launch-profile http-release
```

(`launchSettings.json` viene letto dal progetto; non si applica automaticamente solo copiando i file in `publish`.)

**Porte configurate (riepilogo)**

| Contesto | HTTP | HTTPS | Note |
|----------|------|-------|------|
| Debug (`http` / `https`) | `5165` | `7195` | Ambiente `Development`, Swagger attiva |
| Release locale (`http-release`) | `5080` | `7080` | Ambiente `Production`, Swagger disattivata |
| Porta `5000` | — | — | **Non usata** da queste URL; riservata ad altri usi |

## Swagger / OpenAPI

Swagger è abilitato **solo nell'ambiente `Development`** (vedi `Program.cs`, `app.UseSwagger()` / `app.UseSwaggerUI()`). Una volta avviata l'API:

- Swagger UI:
  - <https://localhost:7195/swagger>
  - <http://localhost:5165/swagger>
- Documento OpenAPI (JSON):
  - <https://localhost:7195/swagger/v1/swagger.json>
  - <http://localhost:5165/swagger/v1/swagger.json>

Se le porte in `launchSettings.json` cambiano, aggiornare di conseguenza i link qui sopra.

## Documentazione canonica (`docs/`)

Sono i file di riferimento per il contratto pubblico dell'API; il codice **deve** restare allineato a questi documenti (e viceversa, quando il modello evolve, vanno aggiornati nello stesso change).

- [`docs/DOMAIN-MODEL.md`](docs/DOMAIN-MODEL.md) — fonte di verità per entità e DTO (nomi, tipi, opzionalità, naming JSON). I `record` in `Plasticfiber.Api/Models/` devono rispecchiarlo.
- [`docs/API-SPEC.md`](docs/API-SPEC.md) — fonte di verità per gli endpoint HTTP (verbo, path, body di request/response, status code). I controller (o eventuali Minimal API) devono rispecchiarlo.

## Postman

La collection Postman v2.1 importabile per i test manuali si trova in:

- [`docs/postman/PlasticFiber-API.postman_collection.json`](docs/postman/PlasticFiber-API.postman_collection.json)

Per importarla: aprire **Postman → Import → File** e selezionare il file. La collection definisce una variabile `baseUrl` che punta di default al profilo `http` locale (`http://localhost:5165`); modificarla a livello di collection o di environment per puntare ad altri host/porte. Per test contro un’istanza avviata in Release con il profilo `http-release` (o con `ASPNETCORE_URLS` sulle stesse porte), impostare `baseUrl` ad esempio su `http://localhost:5080` (o `https://localhost:7080` se si usa HTTPS e il certificato è accettato).

Quando si modificano route, controller, DTO esposti o `docs/API-SPEC.md`, la collection va aggiornata nello stesso PR per restare allineata.

## Cursor rules (`.cursor/rules/`)

Regole automatiche caricate dall'agent Cursor mentre si lavora nel repo:

- [`.cursor/rules/dotnet-web-api.mdc`](.cursor/rules/dotnet-web-api.mdc) — convenzioni .NET 8 specifiche di questo progetto: hosting Minimal in `Program.cs` con `MapControllers()`, layout a cartelle `Controllers/ → Interfaces/ → Services/ → Models/` con namespace allineati, DTO come `record` immutabili, naming `PascalCase` in C# e `camelCase` JSON, `<Nullable>enable</Nullable>` mantenuto, status code documentati in `docs/API-SPEC.md`, sincronizzazione obbligatoria con `docs/DOMAIN-MODEL.md` e `docs/API-SPEC.md`, test di integrazione con xUnit + `WebApplicationFactory<Program>` in `Plasticfiber.Api.Tests`. Si applica solo ai file `Plasticfiber.Api/**/*.cs`.
- [`.cursor/rules/docs-and-specs.mdc`](.cursor/rules/docs-and-specs.mdc) — regola sempre attiva che dichiara come canonici `docs/DOMAIN-MODEL.md` (modello/DTO), `docs/API-SPEC.md` (contratto HTTP) e `docs/postman/PlasticFiber-API.postman_collection.json` (Postman). Impone che ogni modifica a route, DTO, auth o comportamento dell'API venga riflessa nello stesso change su questi documenti, sulla collection Postman e nei test in `Plasticfiber.Api.Tests` (con `dotnet test` da eseguire prima di considerare il lavoro chiuso).

## Cursor skills (`.cursor/skills/`)

Skill di allineamento da invocare esplicitamente in fase di audit o prima del merge:

- [`.cursor/skills/domain-model-alignment/SKILL.md`](.cursor/skills/domain-model-alignment/SKILL.md) — confronta i tipi C# (record/classi) di `Plasticfiber.Api/` con `docs/DOMAIN-MODEL.md` e segnala drift su nomi, tipi, nullability e mapping JSON.
- [`.cursor/skills/api-spec-alignment/SKILL.md`](.cursor/skills/api-spec-alignment/SKILL.md) — confronta le route implementate (controller / Minimal API in `Program.cs`) con `docs/API-SPEC.md` su verbo, path, status code e shape di request/response.
- [`.cursor/skills/postman-collection-alignment/SKILL.md`](.cursor/skills/postman-collection-alignment/SKILL.md) — confronta `docs/postman/PlasticFiber-API.postman_collection.json` con `docs/API-SPEC.md` e con il codice, verificando coerenza di `baseUrl`, header e folder/request della collection.
