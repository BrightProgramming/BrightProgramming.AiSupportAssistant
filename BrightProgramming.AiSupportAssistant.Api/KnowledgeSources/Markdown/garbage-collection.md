# .NET Garbage Collection

## Problem

Applications allocate objects continuously while they run.

Manually tracking the lifetime of every managed object would be error-prone and would make normal application development considerably more difficult.

## Recommended approach

The .NET runtime provides automatic memory management through the garbage collector.

When objects are no longer reachable by the application, their managed memory can eventually be reclaimed by the garbage collector.

Developers therefore normally do not explicitly free managed objects.

## Managed heap

Objects created by managed .NET code are normally allocated on the managed heap.

The garbage collector manages this memory and periodically identifies objects that are no longer reachable.

The memory occupied by those objects can then be reclaimed.

## Generations

The garbage collector uses generations to improve efficiency.

Objects are initially allocated into a young generation.

Objects that survive collections can be promoted to older generations.

The commonly discussed generations are:

- Generation 0
- Generation 1
- Generation 2

Short-lived objects are therefore generally collected more frequently than long-lived objects.

## Large objects

Large allocations are handled differently from typical small allocations.

The Large Object Heap is used for sufficiently large objects.

Large allocations can have different performance characteristics and may require particular attention in applications that create many large objects.

## When garbage collection occurs

Garbage collection is triggered by runtime conditions rather than by a fixed timer.

The runtime considers factors such as allocation activity and available managed memory when deciding when collection is appropriate.

An application should therefore not assume that garbage collection occurs immediately after an object becomes unreachable.

## Object reachability

An object can only be collected when it is no longer reachable from the application's live references.

For example:

```csharp
var customer = new Customer();

customer = null;
```

The original object may now be eligible for collection, assuming no other references exist.

Becoming eligible for collection does not mean that collection happens immediately.

## Garbage collection and Dispose

Garbage collection and deterministic disposal solve different problems.

Garbage collection manages managed memory.

`IDisposable` provides a mechanism for deterministic cleanup of resources.

For example, a database connection may use managed objects internally but also represent an external resource that should be released promptly.

Calling `Dispose` does not force the garbage collector to reclaim the object's memory.

## Memory leaks

Managed applications can still experience memory problems.

An object remains alive if something still references it.

Common causes include:

- Long-lived collections containing objects that are no longer needed.
- Static references retaining objects.
- Event subscriptions that prevent objects from becoming unreachable.
- Caches without appropriate eviction policies.
- Unbounded queues or buffers.

These problems are sometimes described as managed memory leaks.

## Performance considerations

Creating objects is normally inexpensive, but excessive allocation can increase garbage collection activity.

Applications that allocate large numbers of short-lived objects may experience additional GC overhead.

Performance investigations should therefore consider allocation rates and object lifetimes rather than assuming that garbage collection itself is the root cause.

## When to investigate garbage collection

GC behaviour may warrant investigation when:

- Memory usage grows unexpectedly.
- The application experiences frequent collections.
- Large allocations occur regularly.
- Latency increases during periods of heavy allocation.
- Long-lived objects appear to be retaining objects unnecessarily.

## Important distinction

Garbage collection manages managed memory.

It does not replace deterministic cleanup of external resources such as file handles, database connections, or operating system resources.

## Source

Based on concepts documented by Microsoft Learn:

https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals

https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/