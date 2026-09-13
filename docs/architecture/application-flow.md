# Application Flow

The AI Support Assistant follows a simple request flow:

**Web UI → API → Support Service → Knowledge → AI → Response**

## Request flow

1. **User asks a question**

   The user enters a development question in the Blazor web application.

2. **Web application calls the API**

   The question is sent to the ASP.NET Core API.

3. **API calls the Support Service**

   The API delegates the request to `ISupportService`.

4. **Support Service retrieves knowledge**

   The Support Service uses the configured `IKnowledgeProvider` to retrieve relevant knowledge.

5. **Knowledge is provided to the AI**

   The retrieved knowledge and the original question are passed to the configured AI provider.

6. **AI generates the response**

   The AI provider generates an answer using the supplied question and knowledge.

7. **Response is returned**

   The response travels back through the API to the Web application, where it is displayed to the user.

## Separation of responsibilities

The important architectural distinction is:

- **API** — provides the HTTP boundary.
- **Support Service** — coordinates the application workflow.
- **Knowledge Provider** — retrieves and prepares relevant knowledge.
- **AI Provider** — generates the response.
- **Web Application** — presents the interaction to the user.

The Support Service therefore coordinates the workflow without being tightly coupled to the underlying knowledge or AI implementations.

## Extensibility

Because knowledge and AI services are accessed through interfaces, their implementations can be changed independently.

For example, the knowledge implementation could move from Markdown files to a database without changing the request flow.

Similarly, the AI implementation could be replaced with another provider without changing the Web application or API.

See:

- [Provider Model](provider-model.md)
- [Knowledge Architecture](knowledge-architecture.md)