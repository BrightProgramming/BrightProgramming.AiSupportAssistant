# .NET Dependency Injection

## Problem

An application contains services that depend on other services.

Creating those dependencies directly inside each class can tightly couple the implementation to concrete types and make testing and configuration more difficult.

## Symptoms

Common symptoms include:

- Services instantiate their dependencies directly with `new`.
- Classes are difficult to unit test because dependencies cannot easily be replaced.
- A class knows too much about how its dependencies are constructed.
- Service lifetime decisions are scattered throughout application code.
- Changing an implementation requires changes in multiple consumers.

## Recommended approach

Use dependency injection to provide a class with the dependencies it requires.

A service should normally declare its dependencies through its constructor rather than creating them itself.

For example:

```csharp
public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }
}
```

The dependency can then be registered with the application's dependency injection container.

## Service lifetimes

The built-in .NET dependency injection container supports three common service lifetimes.

### Transient

A transient service is created each time it is requested.

Transient services are appropriate when the service is lightweight, stateless, and does not need to be shared.

### Scoped

A scoped service is created once within a particular scope.

In an ASP.NET Core application, a scope normally corresponds to an individual HTTP request.

Scoped services are useful when state should be shared during a request but should not be shared between requests.

### Singleton

A singleton service is created once and reused for the lifetime of the application.

Singletons are appropriate for services that are safe to share and whose state has application-wide lifetime.

Singleton services must be designed with thread safety in mind.

## Design considerations

Dependency injection should not be used simply to hide poor design.

A class with a large number of dependencies may indicate that the class has too many responsibilities.

Services should normally have focused responsibilities and depend on abstractions where appropriate.

## Keyed services

.NET also supports keyed services.

Keyed services are useful when an application has multiple implementations of the same abstraction and the consumer needs to select a particular implementation.

For example, an application might have multiple `ChatClient` instances configured for different AI capabilities.

A key can distinguish those registrations without requiring separate wrapper types for every implementation.

## When to use it

Dependency injection is particularly useful when:

- A service has external dependencies.
- Implementations may need to change.
- Dependencies need to be replaced during testing.
- Service lifetime needs to be controlled centrally.
- Multiple implementations of an abstraction are required.

## Important distinction

Dependency injection is primarily about managing dependencies and their lifetimes.

It does not automatically make an application well designed.

Good separation of responsibilities, appropriate abstractions, and sensible service lifetimes are still required.

## Source

Based on concepts documented by Microsoft Learn:

https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection