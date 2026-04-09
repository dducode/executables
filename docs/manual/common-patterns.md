# Common Patterns

This section collects a few small application-shaped patterns built from the core APIs.

## Build Once, Run with Different Runtimes

One of the main benefits of the model is that executable composition stays reusable while runtime behavior can vary.

```csharp
IExecutable<string, int> parse =
  Executable.Create((string text) => text.Trim())
    .Then(text => int.Parse(text));

IExecutor<string, Result<int>> safe =
  parse
    .GetExecutor()
    .WithPolicy(builder => builder.ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required"))
    .WithResult();

IExecutor<string, Optional<int>> forgiving =
  parse
    .GetExecutor()
    .SuppressException()
    .OfType<FormatException>();
```

The composition is the same in both cases. Only the runtime contract changes.

## Build a Query from Composition

```csharp
IQuery<string, string> normalizeAndFormat =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.ToUpperInvariant())
    .Then(text => $"Value: {text}")
    .AsQuery();
```

This works well when query behavior should stay composable before it is exposed as an application-facing API object.

## Wrap Existing Delegates

```csharp
Func<int, bool> canProcess = value => value > 0;
IExecutable<int, bool> executable = Executable.Create(canProcess);

ICommand<int> command = executable.AsCommand();
Handler<int, bool> handler = executable.AsHandler();
```

This is often the smallest entry point when introducing the library into an existing codebase.

## Add Runtime Rules at the Boundary

```csharp
IExecutor<string, Result<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .Fallback<FormatException>((text, _) => 0))
    .WithResult();
```

This keeps parsing logic in composition and moves validation, recovery, and result shaping to the runtime boundary.

## Route with Explicit Branching

```csharp
IExecutor<int, string> classify =
  Branch.When((int value) => value < 0, Executable.Create((int _) => "negative"))
    .OrWhen(value => value > 0, Executable.Create((int _) => "positive"))
    .OrElse(Executable.Create((int _) => "zero"));
```

This is useful when routing itself should be part of the runtime contract rather than hidden inside one executable body.

## Use Context for Optional Ambient Execution Data

```csharp
IExecutor<int, string> loadUser =
  Executable.Create((int id) =>
  {
    string tenant = ExecutableContext.Current?.GetOrDefault("tenant", "default") ?? "default";
    return $"{tenant}:{id}";
  })
  .GetExecutor()
  .WithContext(context => context.Set("tenant", "eu-1"));
```

This pattern is useful for tenant data, correlation data, tracing tags, and similar ambient execution state when the
executable treats context as optional runtime input rather than assuming it is always present.

## Event-Driven Notification

```csharp
var changed = new Event<string>();

changed.Handle(EventPublisher.Sequential<string>());
changed.Subscribe(value => Console.WriteLine($"A: {value}"));
changed.Subscribe(value => Console.WriteLine($"B: {value}"));

changed.Publish("Updated");
```

This works well when publication and subscription should remain explicit and composable.

## Compose Tuple-Based Enrichment

```csharp
IExecutable<string, string> summary =
  Executable
    .Accumulate((string text) => text.Trim())
    .Accumulate((text, trimmed) => trimmed.Length)
    .Merge((text, trimmed, length) => $"{text} -> {trimmed} ({length})");
```

This is useful when later steps need both the original value and intermediate derived values.

## Promote Sync Composition to Async

```csharp
IExecutable<string, int> parse =
  Executable.Create((string text) => int.Parse(text));

IAsyncExecutable<string, string> asyncPipeline =
  parse.Then(async (int value, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {value}";
  });
```

This pattern is useful when a previously synchronous chain needs to cross an I/O or cancellation boundary without being
rewritten from scratch.
