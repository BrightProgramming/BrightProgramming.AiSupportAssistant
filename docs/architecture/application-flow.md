# Application Flow

The AI Support Assistant follows a simple request pipeline.

A user submits a development question through the web application. The request is handled by the API and coordinated by the `SupportService`.

The Support Service delegates the work to specialised processors rather than directly handling knowledge retrieval or AI integration.

## Request flow

The overall flow is:

**Web Application → API → Support Service → Knowledge Processing → AI Answer Processing → Web Application**

At a high level:

**User → Web → API → Support Service → Knowledge → AI → Response**

## 1. Web Application

The user enters a development question into the web application.

The web application sends the question to the API and displays the resulting answer.

The web application does not need to know how knowledge is retrieved or how the AI response is generated.

## 2. API

The API receives the request from the web application.

It handles the HTTP request and passes the question to the `SupportService`.

The API is responsible for the transport layer rather than the support workflow itself.

## 3. Support Service

`SupportService` coordinates the overall workflow.

It does not directly retrieve files, call OpenAI, or implement knowledge matching.

Instead, it delegates the individual responsibilities to processors.

The main stages are:

- Knowledge processing
- AI answer processing

This keeps the Support Service focused on orchestration.

## 4. Knowledge Processing

The `KnowledgeProcessor` is responsible for obtaining and processing the application's knowledge.

It uses the configured knowledge provider to obtain the available knowledge.

For the current implementation, the knowledge retrieval flow is:

**KnowledgeProcessor → KnowledgeProviderFactory → KnowledgeProvider → KnowledgeRepository → Knowledge Source**

The current concrete implementation is:

**KnowledgeProcessor → MarkdownKnowledgeProvider → MarkdownKnowledgeRepository → Markdown files**

The repository is responsible for reading the underlying Markdown files.

The provider is responsible for supplying that knowledge to the processing pipeline.

For more detail, see [Knowledge Architecture](knowledge-architecture.md).

## 5. Knowledge Matching

Once the knowledge has been obtained, the knowledge processing stage uses the AI knowledge processing abstraction to identify the knowledge relevant to the user's question.

The resulting knowledge context is then passed to the answer processing stage.

Conceptually:

**Question + Knowledge → Relevant Knowledge Context**

This means the AI answer provider does not need to receive the entire knowledge base. It receives the knowledge that has been identified as relevant to the question.

## 6. AI Answer Processing

The `AiAnswerProcessor` is responsible for generating the final answer.

It uses the configured AI answer provider.

The current implementation uses:

**AiAnswerProcessor → AiAnswerProviderFactory → IAiAnswerProvider → OpenAiAnswerProvider → OpenAI**

The AI provider receives the user's question together with the relevant knowledge context and generates the response.

The Support Service does not need to know that OpenAI is being used.

For more information about the provider model, see [Provider Model](provider-model.md).

## 7. Response

The generated answer is returned through the API to the web application.

The web application then displays the response to the user.

The complete conceptual flow is therefore:

**User**

↓

**Web Application**

↓

**API**

↓

**SupportService**

↓

**KnowledgeProcessor**

↓

**KnowledgeProviderFactory**

↓

**Knowledge Provider**

↓

**Knowledge Repository**

↓

**Knowledge**

↓

**AI Knowledge Processing**

↓

**Relevant Knowledge Context**

↓

**AiAnswerProcessor**

↓

**AiAnswerProviderFactory**

↓

**AI Answer Provider**

↓

**OpenAI**

↓

**Answer**

↓

**Web Application**

## Separation of responsibilities

The important architectural principle is that the `SupportService` coordinates the workflow without owning the implementation details.

Each stage has a clear responsibility:

| Component | Responsibility |
|---|---|
| Web Application | Collects questions and displays answers |
| API | Handles HTTP requests |
| SupportService | Coordinates the request workflow |
| KnowledgeProcessor | Obtains and processes knowledge |
| Knowledge Provider | Provides knowledge from a configured source |
| Knowledge Repository | Accesses the underlying knowledge storage |
| AI Knowledge Processing | Identifies relevant knowledge |
| AiAnswerProcessor | Coordinates answer generation |
| AI Answer Provider | Integrates with an AI service |
| OpenAiAnswerProvider | Provides the OpenAI implementation |

## Why the flow is structured this way

The application deliberately separates orchestration from implementation.

For example, `SupportService` does not need to change if:

- Markdown knowledge is replaced by another knowledge source.
- A different knowledge provider is introduced.
- The knowledge processing logic changes.
- OpenAI is replaced by another AI service.
- A different AI answer provider is introduced.

The individual components can therefore evolve independently while the overall request flow remains simple.

## Summary

The application follows a straightforward pipeline:

**Receive → Retrieve Knowledge → Identify Relevant Knowledge → Generate Answer → Return Response**

The `SupportService` coordinates this pipeline while specialised processors, providers, repositories and factories handle the individual responsibilities.

This provides a simple request flow while keeping the implementation replaceable and extensible.