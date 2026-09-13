# Knowledge Architecture

The knowledge system is responsible for finding and preparing information that can be supplied to the AI when answering a question.

The design separates knowledge retrieval into several responsibilities.

## The structure

The current implementation follows this structure:

**Knowledge Provider → Processor → Repository → Knowledge Source**

Each layer has a specific responsibility.

## Knowledge Provider

`IKnowledgeProvider` defines the interface used by the application to retrieve relevant knowledge.

The current implementation is `MarkdownKnowledgeProvider`.

The provider coordinates the knowledge retrieval process but does not need to know the details of how the underlying files are accessed.

This keeps the application independent of the knowledge storage mechanism.

## Knowledge Processor

The Knowledge Processor is responsible for processing knowledge before it is returned to the application.

This provides a place for knowledge-specific processing without putting that responsibility into the provider or repository.

For example, processing could include:

- Filtering relevant content
- Combining multiple documents
- Preparing content for the AI
- Applying knowledge-specific rules

The processing implementation can evolve independently of the application workflow.

## Knowledge Repository

The repository is responsible for accessing the underlying knowledge storage.

The current implementation is `MarkdownKnowledgeRepository`.

It is responsible for reading the Markdown files rather than the `MarkdownKnowledgeProvider`.

This separation means that file-system access is kept out of the provider.

For example:

**MarkdownKnowledgeProvider → MarkdownKnowledgeRepository → Markdown files**

If the storage mechanism changes, the repository implementation can be replaced without changing the Support Service.

## Knowledge Sources

The knowledge source represents the actual information available to the application.

The current implementation uses Markdown files.

This provides a simple and easily maintainable way to store development knowledge in the repository.

Other knowledge sources could be introduced in the future, such as:

- Database records
- REST APIs
- Document stores
- SharePoint
- Vector databases
- Other internal knowledge systems

The application does not need to be tightly coupled to any particular source.

## Why separate the responsibilities?

The separation provides clear boundaries between the different parts of the knowledge system.

- **Provider** — coordinates knowledge retrieval.
- **Processor** — processes the retrieved knowledge.
- **Repository** — accesses the underlying storage.
- **Source** — contains the knowledge.

This follows the same general principle used elsewhere in the application: each component should have a clear responsibility and depend on abstractions where appropriate.

## Current implementation

The current knowledge path is:

**Support Service → MarkdownKnowledgeProvider → Knowledge Processor → MarkdownKnowledgeRepository → Markdown files**

The implementation can therefore start simple while leaving clear extension points for future knowledge sources.

## Extending the knowledge system

A new knowledge implementation can be introduced without changing the Support Service.

For example, a database-backed implementation could provide its own repository and provider while continuing to implement the existing abstractions.

This allows the knowledge system to grow as the requirements of the application change.

## Summary

The knowledge architecture deliberately separates application coordination, knowledge processing and data access.

The current Markdown implementation keeps the system simple while providing clear extension points for alternative knowledge sources.

See [Provider Model](provider-model.md) for how providers are used throughout the application.