# Architecture

The AI Support Assistant is designed around clearly separated responsibilities, with interfaces used to keep key components replaceable and extensible.

## Overview

The application follows a simple flow:

**Web UI → API → Support Service → Knowledge → AI → Response**

![Architecture overview](images/architecture-overview.png)

## Main components

- **Web** — Provides the user interface.
- **API** — Provides the HTTP boundary for the application.
- **Support Service** — Coordinates the support request.
- **Knowledge** — Retrieves and processes internal knowledge.
- **AI** — Matches the question to knowledge and generates the response.

## Design principles

The architecture is based on a few key principles:

- Clear separation of responsibilities
- Interface-driven design
- Replaceable providers
- Separation of application logic from infrastructure
- Configuration-driven implementations

The intention is to make the application easy to understand while allowing individual components to be replaced or extended.

## Detailed architecture

### Application flow

[Application Flow](architecture/application-flow.md) explains how a question moves through the application from the Web UI to the final response.

### Provider model

[Provider Model](architecture/provider-model.md) explains how providers allow implementations to be replaced without changing the application workflow.

### Knowledge architecture

[Knowledge Architecture](architecture/knowledge-architecture.md) explains the relationship between knowledge providers, processors, repositories and knowledge sources.

## Related documentation

- [Knowledge](knowledge.md) — How to use and extend the knowledge system.
- [Configuration](configuration.md) — Application configuration and secrets.
- [Development](development.md) — Setting up, building and running the application.