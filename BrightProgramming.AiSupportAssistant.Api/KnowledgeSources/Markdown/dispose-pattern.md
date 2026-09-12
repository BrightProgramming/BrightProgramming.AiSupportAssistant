# .NET Dispose Pattern

## Problem

Some objects own resources that should be released when the object is no longer required.

Managed memory is automatically reclaimed by the garbage collector, but not every resource is managed memory.

Examples include:

- File handles
- Network resources
- Operating system handles
- Database connections
- Locks
- Other unmanaged resources

## Symptoms

Common symptoms include:

- Files remain locked longer than expected.
- External resources are not released promptly.
- Resource usage increases over time.
- An object owns another IDisposable object but never disposes it.
- Cleanup depends entirely on garbage collection.

## Recommended approach

Use `IDisposable` when a type owns resources that require deterministic cleanup.

A typical implementation exposes:

```csharp
public void Dispose()
{
    // Release resources.
}
```

Consumers can then use the object with a `using` statement.

For example:

```csharp
using var stream = File.OpenRead(path);
```

The compiler ensures that `Dispose` is called when the scope is exited.

## Cascading disposal

If a class owns another object that implements `IDisposable`, the owning class will often need to dispose that dependency when it is responsible for its lifetime.

For example:

```csharp
public sealed class ReportWriter : IDisposable
{
    private readonly StreamWriter _writer;

    public ReportWriter(StreamWriter writer)
    {
        _writer = writer;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
```

Whether a dependency should be disposed depends on ownership.

A class should not dispose an object merely because it happens to reference it if another component owns that object's lifetime.

## Dispose and garbage collection

`Dispose` does not normally mean that the object's managed memory is immediately released.

The garbage collector remains responsible for managed memory.

Dispose is about deterministic cleanup of resources and other state that should be released promptly.

This distinction is important:

```text
Dispose
    → deterministic resource cleanup

Garbage collection
    → automatic reclamation of managed memory
```

## IAsyncDisposable

Some resources require asynchronous cleanup.

Such types can implement `IAsyncDisposable` and provide:

```csharp
public ValueTask DisposeAsync()
{
    // Release resources asynchronously.
}
```

Consumers can use:

```csharp
await using var resource = CreateResource();
```

A type may implement both `IDisposable` and `IAsyncDisposable` when it needs to support both synchronous and asynchronous cleanup.

## Finalizers

Finalizers can provide a fallback mechanism for releasing unmanaged resources when deterministic disposal has not occurred.

However, finalization introduces complexity and should not normally be the first choice for resource management.

Where possible, use safe handles and the standard dispose pattern rather than implementing custom finalization unnecessarily.

## Dependency injection

When a disposable service is registered with the .NET dependency injection container, the container manages its disposal according to the service lifetime.

This means application code should understand ownership rather than manually disposing services that are managed by dependency injection.

## When to use it

Use the dispose pattern when:

- A type owns an IDisposable resource.
- A type owns an IAsyncDisposable resource.
- An unmanaged resource requires deterministic cleanup.
- Resource ownership needs to be clearly defined.

## Important distinction

`IDisposable` is about resource lifetime.

It is not a mechanism for forcing the garbage collector to reclaim an object's memory.

## Source

Based on concepts documented by Microsoft Learn:

https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync