# .NET HttpClient and IHttpClientFactory

## Problem

An application needs to communicate with external HTTP services.

Creating and disposing an HttpClient for every request can result in inefficient connection management and, under high load, can contribute to socket exhaustion.

## Symptoms

Common symptoms include:

- A new HttpClient is created for every HTTP request.
- Connections are frequently created and destroyed.
- Applications experience intermittent connection failures under load.
- Different external services require different HTTP configuration.
- HTTP client configuration is duplicated throughout the application.

## Recommended approach

Reuse HttpClient instances rather than creating and disposing an instance for every request.

For applications that need centrally managed and configurable HTTP clients, use `IHttpClientFactory`.

The factory can create clients with configuration appropriate to a particular external service.

## Named clients

A named client can be configured for a particular purpose.

For example:

```csharp
services.AddHttpClient(
    "CustomerApi",
    client =>
    {
        client.BaseAddress =
            new Uri("https://api.example.com/");
    });
```

A consumer can then request the named client from the factory.

## Typed clients

Typed clients provide a stronger abstraction around an external HTTP service.

For example:

```csharp
public class CustomerApiClient
{
    private readonly HttpClient _httpClient;

    public CustomerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
```

The client can then be registered and configured through `AddHttpClient`.

Typed clients are useful when an application wants a dedicated class representing an external API.

## Connection management

HttpClient maintains connections through its underlying handler.

Repeatedly creating clients can create unnecessary connection pools and may cause connection-management problems.

Long-lived clients can be used with an appropriate `PooledConnectionLifetime`.

Alternatively, `IHttpClientFactory` can manage handlers and provide HttpClient instances while allowing the underlying handlers to be reused.

## Multiple external services

An application that communicates with several APIs can configure separate clients.

For example:

- Customer API
- Payment API
- Reporting API

Each client can have its own:

- Base address
- Default headers
- Authentication configuration
- Timeout
- Delegating handlers
- Resilience configuration

## Resilience

HTTP communication can fail because of transient network or service conditions.

Retry, timeout, and other resilience behaviour should be designed deliberately rather than blindly retrying every failure.

Retries should generally be appropriate to the operation being performed.

## Important distinction

HttpClient itself is not a database connection or a request-scoped object that should normally be created for every request.

Its underlying connection management is one of the reasons that client lifetime needs to be considered carefully.

## When to use it

Use IHttpClientFactory when:

- An application communicates with external HTTP services.
- Multiple differently configured HTTP clients are required.
- HTTP client configuration should be centralised.
- Delegating handlers are required.
- Client creation and handler lifetime should be managed by the application infrastructure.

## Source

Based on concepts documented by Microsoft Learn:

https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines