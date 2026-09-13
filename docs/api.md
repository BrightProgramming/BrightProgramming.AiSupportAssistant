# API

The API is implemented by `SupportController`.

## Endpoint

```text
POST /Support
```

The request body is:

```json
{
  "question": "How does dependency injection work?"
}
```

`Question` is required. ASP.NET Core model validation returns a bad request when the required value is missing.

## Response

A successful response is:

```json
{
  "question": "How does dependency injection work?",
  "answer": "..."
}
```

The controller maps API models to application models through `ISupportApiMapper`.

## Processing

The controller delegates to `ISupportService`.

The service:

1. Gets relevant knowledge for the question.
2. Passes that knowledge to the answer processor.
3. Returns the question and generated answer.

The controller does not perform knowledge retrieval or AI processing.

## Errors

AI provider failures are represented by `AiProviderException` and returned as HTTP `503 Service Unavailable` with Problem Details.

Other unhandled exceptions use HTTP `500 Internal Server Error`.

OpenAPI is mapped only when the API runs in the Development environment.
