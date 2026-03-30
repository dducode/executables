# Handleables and Handlers

## Missing Handler Behavior

Base interaction types do not all behave the same way when no handler is attached:

- `Command<T>` returns `false`,
- `Query<TIn, TOut>` throws `MissingHandlerException`,
- `Event<T>` does nothing when there are no subscribers, but throws when subscribers exist and no publisher handler is attached.

## Subscription Lifetime

Event subscriptions are explicit and disposable.

```csharp
var changed = new Event<string>();
using IDisposable subscription = changed.Subscribe(value => Console.WriteLine(value));
```

One-time subscriptions are supported through `SubscribeOnce(...)`.

## Parameterless Operations

Commands, queries, and events all support parameterless usage through `Unit`.

```csharp
ICommand<Unit> refresh =
  Executable.Create(() => Console.WriteLine("Refresh"))
    .Then(_ => true)
    .AsCommand();
```

## Handleables as Attachment Points

`IHandleable<...>` represents an object that can accept external execution logic through `Handle(...)`.

This is useful when the important thing is not the specific command or query contract, but the fact that the object can
be connected to behavior externally.

Handleables can also be merged so that several attachment points share one handler.

```csharp
IHandleable<string, Unit> first = new Command<string>();
IHandleable<string, Unit> second = new Command<string>();

var merged = first.Merge(second);
merged.Handle(value => Console.WriteLine(value));
```

## Handler Lifecycle

`Handler<TIn, TOut>` and `AsyncHandler<TIn, TOut>` share a common disposal model through `DisposableHandler`.

Important points:

- disposing a handler detaches all active attachments tracked by that handler,
- invoking a disposed handler throws `HandlerDisposedException`,
- `OnDispose(...)` adds custom disposal logic,
- `DisposeOnUnhandledException()` makes unhandled exceptions fatal for the wrapped handler instance.

That disposal model matters most when the same handler is shared between several handleables.
