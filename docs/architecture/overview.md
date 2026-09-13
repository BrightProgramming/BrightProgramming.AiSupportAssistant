# Architecture Overview

The AI Support Assistant separates the web UI, API boundary, application orchestration, knowledge processing and AI integrations.

![Architecture overview](../images/architecture-overview.png)

## Main components

- **Web** — Blazor Web application that collects questions and renders Markdown answers.
- **API** — ASP.NET Core API exposing the `/Support` POST endpoint.
- **SupportService** — Coordinates knowledge retrieval and answer generation.
- **Knowledge processing** — Retrieves available knowledge and asks an AI matcher to identify relevant documents.
- **AI answer processing** — Passes the question and relevant knowledge to the configured answer provider.

## Provider model

Three independent provider abstractions are used:

- `IKnowledgeProvider` — supplies available knowledge.
- `IAiKnowledgeMatcherProvider` — identifies relevant knowledge.
- `IAiAnswerProvider` — generates the final answer.

Each has a corresponding factory that selects the configured implementation.

## Current implementations

The current configuration uses:

- `MarkdownKnowledgeProvider` backed by `MarkdownKnowledgeRepository`.
- `OpenAiKnowledgeMatcherProvider` for knowledge matching.
- `OpenAiAnswerProvider` for answer generation.

The detailed request lifecycle is described in [Application Flow](application-flow.md).

The provider extension model is described in [Provider Model](provider-model.md).

The knowledge implementation is described in [Knowledge Architecture](knowledge-architecture.md).
