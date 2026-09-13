# Development

The solution contains four projects:

- `BrightProgramming.AiSupportAssistant.Api`
- `BrightProgramming.AiSupportAssistant.Web`
- `BrightProgramming.AiSupportAssistant.Api.UnitTests`
- `BrightProgramming.AiSupportAssistant.Api.IntegrationTests`

The API and Web projects both target .NET 10.

## API

The API uses ASP.NET Core controllers, options binding, dependency injection, Problem Details, and the OpenAI client library.

Startup registration is split into focused extension methods:

- `AddOptions`
- `AddMappers`
- `AddAi`
- `AddKnowledge`
- `AddApplicationServices`
- `AddExceptionHandling`

## Web

The web project is a Blazor Web App using Interactive Server components.

The UI is split into components for the hero, question entry and answer rendering.

## Development principle

Keep orchestration separate from provider implementations.

When adding infrastructure, prefer an interface, a concrete implementation, DI registration and configuration rather than coupling the workflow directly to the infrastructure.
