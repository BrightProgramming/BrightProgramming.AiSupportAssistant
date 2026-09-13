# Development

The solution is organised into a Web application, an API and two test projects.

## Applications

`BrightProgramming.AiSupportAssistant.Web` is a Blazor Web application using interactive server components.

`BrightProgramming.AiSupportAssistant.Api` is an ASP.NET Core Web API. Its responsibilities are divided into controllers, services, knowledge processing, AI processing, configuration and startup registration.

## Development principles

The API uses constructor injection and interfaces for application dependencies.

Provider selection is configuration-driven through factories rather than hard-coded in the processors.

Knowledge storage is separated from knowledge provision through `IKnowledgeRepository` and `IKnowledgeProvider`.

AI knowledge matching and answer generation have separate provider abstractions.

## Source layout

See [Project Structure](project-structure.md) for the main folders and classes.

## Extending the system

See [Extending Providers](extending-providers.md) for the provider registration and configuration pattern.

## Testing

The project contains unit tests for controllers, mapping, services, processors, factories, providers and startup configuration.

The API also has integration tests covering successful requests, validation failures, AI provider failures and dependency injection resolution.
