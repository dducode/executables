# Handleables and Handlers

This part of the library is optional.

If your main focus is executable composition, queries, commands, and executor-level runtime behavior, you can read
this section later and come back when you need explicit attachment and registration semantics.

## What This Layer Is For

`IHandleable<TIn, TOut>` represents an attachment point for behavior.

Instead of embedding logic directly into the object, a handleable accepts a `Handler<TIn, TOut>` through `Handle(...)`
and returns an `IDisposable` registration handle.

That model is useful when:

- behavior should be attached and detached explicitly,
- the registration lifetime matters,
- one object exposes a contract and another object supplies the logic,
- multiple attachment points should share the same handler instance.

## Relationship to Executables

A handler is not a separate execution model. `Handler<TIn, TOut>` is also an `IExecutable<TIn, TOut>`.

That means executable logic can be turned into a handler through `AsHandler(...)` and attached to a handleable without
rewriting the implementation.

```csharp
var query = new Query<string, string>();

using IDisposable registration =
  query.Handle(Executable.Create((string text) => text.Trim().ToUpperInvariant()).AsHandler());
```

From that point on, `query.Send(...)` uses the attached handler.

## Single-Handler Registration

The built-in `Handleable<TIn, TOut>` and `AsyncHandleable<TIn, TOut>` base classes are single-handler attachment
points.

They allow one active handler registration at a time. Registering another handler before the first registration is
disposed throws `InvalidOperationException`.

That behavior is often useful when the handleable represents one command endpoint, one query endpoint, or one specific
interaction contract.

## Missing Handler Behavior

Base interaction types do not all react the same way when no handler is attached:

- `Command<T>` returns `false`,
- `Query<TIn, TOut>` throws `MissingHandlerException`,
- `Event<T>` does nothing when there are no subscribers, but throws when subscribers exist and no publisher handler is attached.

This difference reflects the contract shape:

- commands model success/failure execution,
- queries require a result,
- events distinguish between "nothing to publish to" and "publishing path is not configured".

## Registration Lifetime

`Handle(...)` returns an `IDisposable` that controls the registration lifetime.

Dispose that handle when the attachment should be removed:

```csharp
var query = new Query<int, string>();

IDisposable registration =
  query.Handle(Executable.Create((int value) => value.ToString()).AsHandler());

string text = query.Send(42);
registration.Dispose();

// query.Send(42); throws MissingHandlerException
```

This makes attachment explicit and easy to scope with `using`.

## Merging Attachment Points

Several handleables with the same contract can be merged into one registration target.

This is useful when one handler should be attached to multiple sources at once.

```csharp
IHandleable<string, Unit> first = new Command<string>();
IHandleable<string, Unit> second = new Command<string>();

IHandleable<string, Unit> merged = first.Merge(second);

using IDisposable registration =
  merged.Handle(value => Console.WriteLine(value));
```

The returned registration detaches the handler from all merged attachment points together.

## Creating Custom Handleables

`Handleable.Create(...)` and `AsyncHandleable.Create(...)` let you build attachment points directly from registration
logic.

This is useful when the important thing is the subscription model itself, not a predefined query or command type.

There are also `Handleable.FromEvent(...)` overloads for adapting ordinary .NET events into handleables with explicit
registration and unregistration behavior.

## Handler Lifecycle

`Handler<TIn, TOut>` and `AsyncHandler<TIn, TOut>` share a disposal model through `DisposableHandler`.

Important points:

- disposing a handler detaches all active registrations tracked by that handler,
- invoking a disposed handler throws `HandlerDisposedException`,
- `OnDispose(...)` adds custom disposal logic,
- `DisposeOnUnhandledException()` makes unhandled exceptions fatal for the wrapped handler instance.

That lifecycle matters most when a handler instance is shared across several handleables or when registration and
cleanup need to be coordinated explicitly.

## Parameterless Contracts

Handleables, handlers, commands, and queries also support parameterless forms through `Unit`.

```csharp
ICommand<Unit> refresh =
  Executable.Create(() => Console.WriteLine("Refresh"))
    .Then(_ => true)
    .AsCommand();
```

This keeps the contract explicit even when no meaningful input value is required.
