# Extending providers

The provider model is designed to allow alternative implementations.

There are three provider interfaces to consider.

## Knowledge

Implement `IKnowledgeProvider`.

If storage access needs its own abstraction, implement `IKnowledgeRepository` as well.

Register the provider and repository with dependency injection and configure the provider name through `KnowledgeSource:Provider`.

## Knowledge matching

Implement `IAiKnowledgeMatcherProvider`.

Register it with dependency injection and configure `AiKnowledgeMatcher:Provider` with its `Name`.

The provider receives the question and complete available `KnowledgeContext`.

## Answer generation

Implement `IAiAnswerProvider`.

Register it with dependency injection and configure `AiAnswer:Provider` with its `Name`.

The provider receives the original question and matched `KnowledgeContext`.

## Important detail

Provider factories use `SingleOrDefault` against the `Name` property.

Provider names are compared case-insensitively.

If the configured name has no registered match, or more than one registered provider matches, factory resolution fails with `InvalidOperationException`.

Provider-specific clients and options should be kept behind the provider implementation where practical.
