# Testing

The solution contains unit tests and API integration tests.

## Unit tests

The unit-test project covers:

- `SupportController`
- `SupportService`
- `SupportApiMapper`
- Knowledge providers, processors and factories
- AI answer and knowledge-matcher processors, providers and factories
- Startup configuration and options validation
- Exception handling

Provider tests use mocked `ChatClient` instances where appropriate.

## Integration tests

`SupportApiIntegrationTests` uses `WebApplicationFactory<Program>` to exercise the API endpoint.

The tests replace `IKnowledgeProcessor` and `IAiAnswerProcessor` with stubs so the endpoint can be tested without making real AI requests.

Covered scenarios include:

- Successful POST returning the mapped question and answer.
- Invalid requests returning `400 Bad Request` problem details.
- `AiProviderException` returning `503 Service Unavailable` with an appropriate problem-details message.
- Resolving application services from dependency injection.

## Running tests

The test projects target .NET 10 and can be run using the normal .NET test tooling or from the test runner in an IDE such as Visual Studio.
