# AI Support Assistant

An AI-powered support assistant for answering development questions using internal knowledge and AI.

## The application

The AI Support Assistant provides a simple web interface for asking development questions and receiving answers based on the project's configured knowledge and AI services.

![AI Support Assistant](docs/images/ai-support-assistant.png)

The application retrieves the available knowledge, uses AI to identify the relevant documents, and then uses AI again to generate the answer.

## Key features

- **Internal knowledge** — Markdown documents provide the current knowledge source.
- **AI knowledge matching** — OpenAI identifies the documents relevant to a question.
- **AI answers** — OpenAI generates an answer using the matched knowledge.
- **Replaceable providers** — Knowledge, matching and answer providers are accessed through interfaces and factories.
- **Simple web UI** — A Blazor Web application provides the question-and-answer experience.

## Documentation

- [Architecture](docs/architecture/overview.md) — How the application is structured.
- [Getting started](docs/getting-started/overview.md) — Build, configure and run the application.
- [Knowledge](docs/knowledge.md) — Manage and extend the Markdown knowledge source.
- [Development](docs/development/overview.md) — Project structure, testing and extension points.
