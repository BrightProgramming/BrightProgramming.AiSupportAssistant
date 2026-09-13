# Knowledge Architecture

The knowledge subsystem separates storage access, knowledge provision and AI-based relevance matching.

![Knowledge flow](../images/knowledge-flow.png)

## Storage

`IKnowledgeRepository` defines access to the underlying knowledge store.

The current `MarkdownKnowledgeRepository`:

- Resolves the configured path to a full path.
- Recursively finds `*.md` files.
- Reads each file as text.
- Creates a `KnowledgeDocument` containing the file name and content.

If the directory does not exist, it logs a warning and returns an empty collection.

## Knowledge provider

`IKnowledgeProvider` exposes `GetKnowledgeAsync` and supplies a `KnowledgeContext`.

`MarkdownKnowledgeProvider` delegates document loading to `IKnowledgeRepository` and wraps the documents in a `KnowledgeContext`.

It uses `Lazy<KnowledgeContext>`, so the documents are loaded once and the same context is returned on subsequent calls.

## Knowledge processor

`KnowledgeProcessor` obtains the configured provider through `IKnowledgeProviderFactory`.

It retrieves the available knowledge and passes it, together with the question, to `IAiKnowledgeProcessor`.

The processor returns the context produced by the matcher.

## AI knowledge matching

`AiKnowledgeProcessor` obtains `IAiKnowledgeMatcherProvider` from `IAiKnowledgeMatcherFactory`.

`OpenAiKnowledgeMatcherProvider` sends the question and all available documents to OpenAI. It expects JSON containing relevant document names, then filters the original collection by those names.

The result is a new `KnowledgeContext` containing only matched documents.

## Current pipeline

**Markdown files → MarkdownKnowledgeRepository → MarkdownKnowledgeProvider → KnowledgeProcessor → AI matcher → Relevant KnowledgeContext**

The repository therefore does not decide relevance, and the AI answer provider does not need to receive the entire knowledge store.
