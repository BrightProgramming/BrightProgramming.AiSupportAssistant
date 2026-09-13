# Knowledge Architecture

The knowledge system is responsible for retrieving the application's internal knowledge and identifying the information that is relevant to a user's question.

The design separates knowledge retrieval, storage access and knowledge processing so that each responsibility can evolve independently.

## The structure

The current knowledge flow is:

**KnowledgeProcessor → KnowledgeProvider → KnowledgeRepository → Knowledge Source**

The retrieved knowledge is then processed to identify the information relevant to the user's question.

Conceptually:

**Question → Retrieve Knowledge → Identify Relevant Knowledge → Knowledge Context**

## Knowledge Processor

`KnowledgeProcessor` coordinates the knowledge processing workflow.

It obtains the configured knowledge provider through the knowledge provider factory and uses that provider to retrieve the available knowledge.

Once the knowledge has been retrieved, the processor passes it to the AI knowledge processing abstraction to identify the knowledge relevant to the user's question.

The processor therefore sits between the application workflow and the knowledge provider.

Its responsibilities include:

- Obtaining the configured knowledge provider.
- Retrieving the available knowledge.
- Passing the knowledge to the knowledge processing implementation.
- Returning the relevant knowledge context.

The processor does not need to know how the underlying knowledge is stored.

## Knowledge Provider

`IKnowledgeProvider` defines the abstraction used to obtain knowledge.

The current implementation is `MarkdownKnowledgeProvider`.

The provider is responsible for supplying knowledge to the processing pipeline, but it does not directly access the file system.

The current implementation delegates storage access to `IKnowledgeRepository`.

This gives us:

**IKnowledgeProvider → IKnowledgeRepository**

For the current Markdown implementation:

**MarkdownKnowledgeProvider → MarkdownKnowledgeRepository**

This separation allows a different knowledge source to be introduced without changing the rest of the knowledge processing pipeline.

## Knowledge Repository

`IKnowledgeRepository` defines the abstraction for accessing the underlying knowledge storage.

The current implementation is `MarkdownKnowledgeRepository`.

The repository is responsible for reading the Markdown files from the configured knowledge location.

This keeps file-system access out of `MarkdownKnowledgeProvider`.

The current implementation therefore follows:

**MarkdownKnowledgeProvider → MarkdownKnowledgeRepository → Markdown files**

The repository can be replaced if the underlying storage mechanism changes.

For example, a future implementation could retrieve knowledge from:

- A database
- A document store
- A REST API
- SharePoint
- Another internal knowledge system

The provider does not need to know how that storage is implemented.

## Knowledge Source

The knowledge source contains the actual information available to the application.

The current implementation uses Markdown files.

Markdown provides a simple and maintainable way to store development knowledge alongside the application source code.

The files can be version controlled using Git and updated independently of the application code.

The storage location is configurable, so the application does not need to assume a particular physical directory.

## Knowledge Processing

Retrieving knowledge and identifying relevant knowledge are deliberately separate responsibilities.

The repository retrieves the available knowledge.

The provider makes that knowledge available to the processing pipeline.

The knowledge processor then uses the AI knowledge processing abstraction to identify the information that is relevant to the user's question.

Conceptually:

**Knowledge Source → Knowledge Provider → Knowledge Processor → Relevant Knowledge**

This prevents the repository or provider from becoming responsible for deciding which information is relevant to a particular question.

## Current implementation

The current Markdown implementation follows this path:

**SupportService**

↓

**KnowledgeProcessor**

↓

**IKnowledgeProviderFactory**

↓

**IKnowledgeProvider**

↓

**MarkdownKnowledgeProvider**

↓

**IKnowledgeRepository**

↓

**MarkdownKnowledgeRepository**

↓

**Markdown files**

The retrieved knowledge then passes through the AI knowledge processing stage to produce the relevant knowledge context used when generating the answer.

## Why separate Provider and Repository?

The separation between the provider and repository is intentional.

The provider represents the application's knowledge abstraction.

The repository represents access to the underlying storage.

For example, `MarkdownKnowledgeProvider` should not need to know whether Markdown files are being read from a local directory, a mounted volume or another storage mechanism.

That responsibility belongs to `MarkdownKnowledgeRepository`.

This gives each component a clear responsibility:

| Component | Responsibility |
|---|---|
| `KnowledgeProcessor` | Coordinates knowledge retrieval and processing |
| `IKnowledgeProvider` | Defines how knowledge is provided |
| `MarkdownKnowledgeProvider` | Provides knowledge using the Markdown implementation |
| `IKnowledgeRepository` | Defines access to knowledge storage |
| `MarkdownKnowledgeRepository` | Reads Markdown knowledge from storage |
| Markdown files | Contain the actual knowledge |

## Extending the knowledge system

The current implementation deliberately keeps the knowledge source simple.

If the application needs to support another source in the future, new implementations can be introduced behind the existing abstractions.

For example:

**DatabaseKnowledgeRepository**

could provide knowledge from a database while the rest of the application continues to depend on `IKnowledgeRepository`.

Similarly, a new knowledge provider could implement `IKnowledgeProvider` if the way knowledge is supplied to the processing pipeline needs to change.

The `SupportService` does not need to know which concrete implementation is being used.

## Benefits

This architecture provides several benefits:

- **Separation of concerns** — retrieval, storage access and processing have distinct responsibilities.
- **Replaceability** — the Markdown implementation can be replaced without changing the application workflow.
- **Testability** — interfaces can be replaced with test implementations.
- **Extensibility** — new knowledge sources can be introduced as requirements evolve.
- **Maintainability** — file-system and storage concerns are kept out of the application workflow.
- **Version control** — the current Markdown knowledge can be maintained alongside the source code.

## Summary

The knowledge architecture deliberately separates the responsibilities of processing, providing and storing knowledge.

The current implementation is:

**KnowledgeProcessor → MarkdownKnowledgeProvider → MarkdownKnowledgeRepository → Markdown files**

The retrieved knowledge is then processed to identify the information relevant to the user's question.

This allows the application to start with a simple Markdown-based knowledge source while providing clear extension points for future knowledge stores and providers.

See [Application Flow](application-flow.md) for the complete request lifecycle.

See [Provider Model](provider-model.md) for more information about replacing and extending providers.