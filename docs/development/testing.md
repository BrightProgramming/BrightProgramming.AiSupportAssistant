# Testing

The API has both unit and integration tests.

## Unit tests

The unit-test project covers:

- Controllers and model mapping
- `SupportService`
- Knowledge processing
- Markdown knowledge loading and caching
- Provider factories
- AI processors and providers
- Options validation
- Dependency-injection registration
- Exception handling

Provider tests cover successful calls, provider failures, cancellation, and client failures.

## Integration tests

`SupportApiIntegrationTests` runs the API through `WebApplicationFactory`.

The tests replace the real knowledge and answer processors with stubs so the HTTP pipeline can be exercised without calling OpenAI.

Covered scenarios include:

- Successful `POST /Support`
- Invalid request returning `400`
- `AiProviderException` returning `503` Problem Details
- Application services resolving from dependency injection

## Running tests

From the repository root:

```text
dotnet test
```

The test projects target .NET 10.
