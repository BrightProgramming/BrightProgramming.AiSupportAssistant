# Application Flow

The AI Support Assistant follows a simple request pipeline.

A user submits a development question through the web application. The request is handled by the API and coordinated by the `SupportService`.

The Support Service delegates the work to specialised processors rather than directly handling knowledge retrieval, knowledge matching or AI answer generation.

## Request flow

The overall flow is:

**Web Application → API → Support Service → Knowledge Processing → AI Answer Processing → Web Application**

At a high level:

**User → Web → API → Support Service → Knowledge → AI → Response**

## 1. Web Application

The user enters a development question into the web application.

The web application sends the question to the API and displays the resulting answer.

The web application does not need to know how knowledge is retrieved, how relevant knowledge is identified, or how the AI response is generated.

## 2. API

The API receives the request from the web application through `SupportController`.

The controller:

1. Receives the API request.
2. Maps it to the application's `SupportRequest`.
3. Passes the request to `ISupportService`.
4. Maps the resulting `SupportResponse` back to the API response.

The API is responsible for the HTTP boundary rather than the support workflow itself.

## 3. Support Service

`SupportService` coordinates the overall workflow.

It depends on two processors:

- `IKnowledgeProcessor`
- `IAiAnswerProcessor`

The Support Service does not directly retrieve files, perform knowledge matching, call OpenAI, or select concrete providers.

Instead, it:

1. Requests relevant knowledge for the question.
2. Passes that knowledge to the AI answer processor.
3. Returns the resulting answer.

This keeps the Support Service focused on orchestration.

## 4. Knowledge Processing

`KnowledgeProcessor` is responsible for obtaining and processing the application's knowledge.

It obtains the configured knowledge provider through `IKnowledgeProviderFactory`.

The factory selects the provider based on configuration.

The current implementation therefore follows:

**KnowledgeProcessor → IKnowledgeProviderFactory → IKnowledgeProvider → MarkdownKnowledgeProvider → IKnowledgeRepository → MarkdownKnowledgeRepository → Markdown files**

`MarkdownKnowledgeRepository` is responsible for reading the Markdown files.

`MarkdownKnowledgeProvider` converts the retrieved documents into a `KnowledgeContext` and caches that context for subsequent requests.

For more detail, see [Knowledge Architecture](knowledge-architecture.md).

## 5. Knowledge Matching

Once the knowledge has been retrieved, `KnowledgeProcessor` passes it to `IAiKnowledgeProcessor`.

The current `AiKnowledgeProcessor` obtains its configured `IAiKnowledgeMatcherProvider` through `IAiKnowledgeMatcherFactory`.

The current implementation uses `OpenAiKnowledgeMatcherProvider`.

The matcher receives:

- The user's question.
- The available knowledge documents.

It asks the AI service to identify which documents are relevant and returns a new `KnowledgeContext` containing only the matched documents.

Conceptually:

**Question + Available Knowledge → Relevant Knowledge**

This is an important part of the request pipeline because the answer-generation stage does not need to receive the entire knowledge base.

## 6. AI Answer Processing

The resulting `KnowledgeContext` is passed to `AiAnswerProcessor`.

`AiAnswerProcessor` obtains its configured answer provider through `IAiAnswerProviderFactory`.

The current implementation is:

**IAiAnswerProviderFactory → IAiAnswerProvider → OpenAiAnswerProvider → OpenAI**

The answer provider receives:

- The original question.
- The relevant `KnowledgeContext`.

It then generates the final answer using the configured AI service.

The Support Service does not need to know that OpenAI is being used.

For more information about the provider model, see [Provider Model](provider-model.md).

## 7. Response

The generated answer is returned through the application in the reverse direction:

**OpenAI → OpenAiAnswerProvider → AiAnswerProcessor → SupportService → SupportController → Web Application**

The Web application then renders the response for the user.

The answer is returned as Markdown and is rendered by the Web application so that headings, lists, inline code and code blocks can be displayed appropriately.

## Complete flow

The complete current implementation can be summarised as:

**User**

↓

**Web Application**

↓

**SupportController**

↓

**SupportService**

↓

**KnowledgeProcessor**

↓

**KnowledgeProviderFactory**

↓

**MarkdownKnowledgeProvider**

↓

**MarkdownKnowledgeRepository**

↓

**Markdown files**

↓

**AI Knowledge Processor**

↓

**AI Knowledge Matcher Factory**

↓

**OpenAiKnowledgeMatcherProvider**

↓

**Relevant KnowledgeContext**

↓

**AiAnswerProcessor**

↓

**AI Answer Provider Factory**

↓

**OpenAiAnswerProvider**

↓

**OpenAI**

↓

**Answer**

↓

**Web Application**

## Separation of responsibilities

The important architectural principle is that the `SupportService` coordinates the workflow without owning the implementation details.

| Component | Responsibility |
|---|---|
| Web Application | Collects questions and displays answers |
| SupportController | Handles the HTTP endpoint and maps API models |
| SupportService | Coordinates the support workflow |
| KnowledgeProcessor | Obtains knowledge and coordinates knowledge matching |
| KnowledgeProviderFactory | Selects the configured knowledge provider |
| MarkdownKnowledgeProvider | Provides knowledge from the Markdown implementation |
| MarkdownKnowledgeRepository | Reads Markdown documents from storage |
| AiKnowledgeProcessor | Coordinates AI-based knowledge matching |
| AiKnowledgeMatcherFactory | Selects the configured knowledge matcher |
| OpenAiKnowledgeMatcherProvider | Uses OpenAI to identify relevant documents |
| AiAnswerProcessor | Coordinates answer generation |
| AiAnswerProviderFactory | Selects the configured AI answer provider |
| OpenAiAnswerProvider | Uses OpenAI to generate the final answer |

## Why the flow is structured this way

The application deliberately separates orchestration, knowledge retrieval, knowledge matching and answer generation.

For example, `SupportService` does not need to change if:

- The Markdown knowledge source is replaced by another knowledge source.
- A different knowledge provider is introduced.
- A different knowledge matcher is introduced.
- The knowledge matching implementation changes.
- OpenAI is replaced by another AI service.
- A different AI answer provider is introduced.

The individual components can therefore evolve independently while the overall request flow remains simple.

## Summary

The application follows a straightforward pipeline:

**Receive → Retrieve Knowledge → Identify Relevant Knowledge → Generate Answer → Return Response**

The `SupportService` coordinates this pipeline while specialised processors, factories, providers and repositories handle the individual responsibilities.

This provides a simple request flow while keeping the implementation replaceable and extensible.