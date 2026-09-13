# Project Structure

The solution contains four projects.

| Project | Purpose |
|---|---|
| `BrightProgramming.AiSupportAssistant.Api` | ASP.NET Core API and application logic |
| `BrightProgramming.AiSupportAssistant.Web` | Blazor Web user interface |
| `BrightProgramming.AiSupportAssistant.Api.UnitTests` | API unit tests |
| `BrightProgramming.AiSupportAssistant.Api.IntegrationTests` | API integration tests |

## API structure

The main API areas are:

- `Controllers` — HTTP endpoints.
- `Services` — application orchestration.
- `Knowledge` — providers, repository, models and processing.
- `Ai/Answer` — answer processor, factory and providers.
- `Ai/Matcher` — knowledge-matching processor, factory and providers.
- `Configuration` — strongly typed options.
- `StartupConfiguration` — dependency injection and middleware registration.
- `Mappers` — API/application model mapping.
- `KnowledgeSources/Markdown` — current knowledge documents.

## Web structure

The Web project contains Razor components under `Components`, a small HTTP service under `Services`, and static assets under `wwwroot`.

The main page is `Components/Pages/Support.razor`.

## Tests

Unit tests mirror the API's major responsibilities. Integration tests use `WebApplicationFactory` and replace the knowledge and answer processors with test implementations.
