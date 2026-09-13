# Extending Providers

The provider model is designed to allow alternative implementations without changing the support workflow.

## Knowledge provider

Implement `IKnowledgeProvider` and provide a `Name` that identifies the implementation.

Register the implementation with dependency injection and configure `KnowledgeSource:Provider` with the same name.

If the provider needs storage access, it can depend on `IKnowledgeRepository` or another appropriate abstraction.

## Knowledge matcher

Implement `IAiKnowledgeMatcherProvider` with a unique `Name`.

Register it with dependency injection and configure `AiKnowledgeMatcher:Provider` to select it.

The provider receives the user's question and the available `KnowledgeContext` and returns the relevant context.

## Answer provider

Implement `IAiAnswerProvider` with a unique `Name`.

Register it with dependency injection and configure `AiAnswer:Provider` to select it.

The provider receives the original question and the matched `KnowledgeContext` and returns the answer text.

## Factory behaviour

Each factory receives `IEnumerable<TProvider>` from dependency injection and selects the provider whose `Name` matches the configured value, ignoring case.

No matching provider causes an `InvalidOperationException`. More than one matching provider also causes an exception.

## AI clients

The current OpenAI implementation uses two keyed `ChatClient` registrations: one for answer generation and one for knowledge matching.

A replacement implementation can use different dependencies without changing the processor or `SupportService` contracts.
