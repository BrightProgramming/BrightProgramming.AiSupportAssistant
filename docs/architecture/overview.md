# Architecture overview

The application separates the user interface, HTTP boundary, workflow orchestration, knowledge processing, and AI integrations.

![Architecture overview](../images/architecture-overview.png)

## Main components

- **Web application** — Blazor Web App using Interactive Server rendering.
- **API** — ASP.NET Core controller-based API.
- **SupportService** — coordinates the support workflow.
- **KnowledgeProcessor** — retrieves knowledge and invokes knowledge matching.
- **AiKnowledgeProcessor** — delegates relevance matching to a provider.
- **AiAnswerProcessor** — delegates answer generation to a provider.

## Provider boundaries

Three provider abstractions are deliberately separate:

- `IKnowledgeProvider`
- `IAiKnowledgeMatcherProvider`
- `IAiAnswerProvider`

Factories select the configured implementation for each abstraction.

## Current implementations

The current source provides:

- `MarkdownKnowledgeProvider`
- `OpenAiKnowledgeMatcherProvider`
- `OpenAiAnswerProvider`

Dependency injection registers these implementations as singletons.

The `SupportService` is scoped.

## Key principle

The core workflow works with processors and abstractions rather than concrete knowledge or AI implementations.

This makes the three infrastructure responsibilities independently replaceable without requiring the `SupportService` to know which concrete provider is selected.
