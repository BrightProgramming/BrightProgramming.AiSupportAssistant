# Application Flow

A support request passes through the web application, API, support service, knowledge processing and AI answer processing.

![Application flow](../images/application-flow.png)

## 1. Web application

The Blazor Web application collects the question and calls its named `HttpClient` against `/Support`.

The response contains the original question and generated answer. `SupportAnswer` renders the answer as Markdown using Markdig with HTML disabled.

## 2. API

`SupportController` receives the POST request and uses `SupportApiMapper` to convert the API model into `SupportRequest`.

It calls `ISupportService`, maps the resulting `SupportResponse` to `AskResponse`, and returns `200 OK`.

## 3. Support service

`SupportService` coordinates two processors:

- `IKnowledgeProcessor`
- `IAiAnswerProcessor`

It first requests relevant knowledge, then passes that context and the original question to the answer processor.

## 4. Knowledge processing

`KnowledgeProcessor` obtains its provider from `IKnowledgeProviderFactory` and calls `GetKnowledgeAsync`.

The current provider is `MarkdownKnowledgeProvider`, which obtains documents through `MarkdownKnowledgeRepository`.

The provider caches the resulting `KnowledgeContext` after its first load.

## 5. Knowledge matching

`KnowledgeProcessor` passes the available knowledge to `IAiKnowledgeProcessor`.

`AiKnowledgeProcessor` obtains `IAiKnowledgeMatcherProvider` from `IAiKnowledgeMatcherFactory`. The current provider is `OpenAiKnowledgeMatcherProvider`.

The matcher asks OpenAI to select document names relevant to the question and returns a new `KnowledgeContext` containing only matching documents.

## 6. Answer generation

`AiAnswerProcessor` obtains `IAiAnswerProvider` from `IAiAnswerProviderFactory`.

`OpenAiAnswerProvider` sends the original question and the relevant knowledge to OpenAI and returns the generated text.

The provider instructs the model to use only the supplied knowledge and to say when it is insufficient rather than inventing an answer.

## 7. Response

The answer travels back through `SupportService` and `SupportController` to the web application.

The web application renders the returned Markdown as HTML with raw HTML disabled.

## Summary

The runtime flow is:

**Question → API → SupportService → Retrieve Knowledge → Match Knowledge → Generate Answer → Response**

The orchestration code does not select concrete providers directly; the processor factories resolve the configured implementations.
