# .NET Logging

## Problem

Applications need to record important events so that their behaviour can be understood during development, testing, and production operation.

Writing directly to the console throughout an application makes logging difficult to control and integrate with different logging systems.

## Symptoms

Common symptoms include:

- Important application events cannot be diagnosed after deployment.
- Logging is implemented using `Console.WriteLine` throughout the application.
- Log messages contain inconsistent information.
- Sensitive information is accidentally written to logs.
- Logs cannot easily be filtered by severity or category.

## Recommended approach

Use the .NET logging abstractions provided by `Microsoft.Extensions.Logging`.

The primary abstraction is `ILogger<T>`.

A service can receive an `ILogger<T>` through dependency injection:

```csharp
public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        ILogger<PaymentService> logger)
    {
        _logger = logger;
    }
}
```

The logger category is normally based on the type associated with the logger.

## Log levels

.NET logging provides several standard log levels.

### Trace

Very detailed information intended primarily for diagnosing complex problems.

### Debug

Information useful during development and troubleshooting.

### Information

Normal application events that provide useful operational information.

### Warning

An unexpected condition that does not necessarily prevent the operation from completing.

### Error

A failure that prevented an operation from completing successfully.

### Critical

A serious failure that requires immediate attention.

## Structured logging

Prefer structured logging rather than constructing messages entirely as strings.

For example:

```csharp
_logger.LogInformation(
    "Processing order {OrderId} for customer {CustomerId}",
    orderId,
    customerId);
```

The values can then be captured as structured properties by the logging system.

This makes logs easier to search, filter, and analyse.

## Logging exceptions

When logging an exception, pass the exception separately to the logging API.

For example:

```csharp
_logger.LogError(
    exception,
    "Failed to process order {OrderId}",
    orderId);
```

This preserves the exception information for logging providers.

## Configuration

Logging levels can be configured without changing application code.

For example, an ASP.NET Core application can configure different minimum levels for the application and framework namespaces.

## Security considerations

Logs should not contain secrets or sensitive information unless there is a specific and controlled reason to record it.

Examples of information that should normally not be logged include:

- API keys
- Passwords
- Authentication tokens
- Payment credentials
- Personal data that is not required for diagnosis

## When to use it

Logging is particularly useful when:

- An application operates outside the developer's environment.
- Failures need to be diagnosed after deployment.
- Important business or technical events need to be observed.
- Operational behaviour needs to be investigated.
- Applications integrate with external services.

## Important distinction

Logging records information about application behaviour.

It should not be treated as the primary mechanism for application state, business data, or audit requirements unless the logging design explicitly satisfies those requirements.

## Source

Based on concepts documented by Microsoft Learn:

https://learn.microsoft.com/en-us/dotnet/core/diagnostics/logging-tracing