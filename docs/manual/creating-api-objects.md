# Creating API Objects

## Creating Executables

`Executable.Create(...)` is the main sync entry point. `AsyncExecutable.Create(...)` is the async counterpart.

```csharp
IExecutable<int, int> square =
  Executable.Create((int x) => x * x);

IAsyncExecutable<int, int> doubleAsync =
  AsyncExecutable.Create(async (int x, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return x * 2;
  });
```

For multi-argument delegates, input is represented as a tuple.

```csharp
IExecutable<(int, int), int> sum =
  Executable.Create((int a, int b) => a + b);
```

## Identity Executables

`Executable.Identity<T>()` and `AsyncExecutable.Identity<T>()` create pass-through executables.

```csharp
IExecutable<int, int> identity = Executable.Identity<int>();
```

## Converting to Queries, Commands, and Handlers

Reusable executable logic can be exposed through interaction-oriented APIs.

```csharp
IQuery<int, string> getUser =
  Executable.Create((int id) => $"User-{id}")
    .AsQuery();

ICommand<string> saveName =
  Executable.Create((string value) => !string.IsNullOrWhiteSpace(value))
    .AsCommand();

Handler<int, string> formatHandler =
  Executable.Create((int id) => $"User-{id}")
    .AsHandler();
```

## Creating Events

Events are created explicitly and then connected to subscribers and a publisher strategy.

```csharp
var changed = new Event<string>();

changed.Subscribe(value => Console.WriteLine(value));
changed.Handle(EventPublisher.Sequential<string>());

changed.Publish("Updated");
```
