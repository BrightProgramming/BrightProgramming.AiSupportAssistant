# Provider model

The application has three independent provider abstractions.

![Provider model](../images/provider-model.png)

## Knowledge provider

`IKnowledgeProvider` supplies a `KnowledgeContext`.

The factory is `IKnowledgeProviderFactory`.

The current implementation is `MarkdownKnowledgeProvider`, backed by `IKnowledgeRepository` and `MarkdownKnowledgeRepository`.

## Knowledge matcher provider

`IAiKnowledgeMatcherProvider` receives a question and available knowledge and returns a `KnowledgeContext` containing the documents selected as relevant.

The factory is `IAiKnowledgeMatcherFactory`.

The current implementation is `OpenAiKnowledgeMatcherProvider`.

## Answer provider

`IAiAnswerProvider` receives the question and relevant knowledge and returns the generated answer.

The factory is `IAiAnswerProviderFactory`.

The current implementation is `OpenAiAnswerProvider`.

## Selection

Each factory receives all registered providers plus its corresponding options object.

The configured provider name is matched against the provider's `Name` property, ignoring case.

An unregistered name, or multiple matching providers, causes the factory construction to fail.

## Extensibility

A new implementation must:

1. Implement the relevant provider interface.
2. Register it with dependency injection.
3. Give it the configured provider name.
4. Add the required configuration and supporting infrastructure.

The relevant processor and `SupportService` do not need to know the concrete implementation.
