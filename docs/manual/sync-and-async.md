# Sync and Async

The library keeps sync and async APIs structurally aligned.

`IExecutable<TIn, TOut>` and `IAsyncExecutable<TIn, TOut>` describe the same kind of composable contract. The main
difference is only the execution model:

- sync execution returns `TOut`,
- async execution returns `ValueTask<TOut>`.

The same parallel shape exists for:

- executors,
- queries,
- commands,
- handlers,
- branching,
- tuple-based composition helpers.

## Sync First, Async When Needed

A sync executable can always be promoted to an async one through `ToAsyncExecutable()`.

```csharp
IExecutable<int, int> square = Executable.Create((int x) => x * x);
IAsyncExecutable<int, int> squareAsync = square.ToAsyncExecutable();
```

The same idea applies to higher-level API objects:

```csharp
IQuery<string, int> query =
  Executable.Create((string text) => int.Parse(text))
    .AsQuery();

IAsyncQuery<string, int> queryAsync = query.ToAsyncQuery();
```

```csharp
ICommand<string> command =
  Executable.Create((string value) => value.Length > 0)
    .AsCommand();

IAsyncCommand<string> commandAsync = command.ToAsyncCommand();
```

This makes it practical to start with sync composition and move to async only when execution actually needs it.

## Mixed Composition

Sync and async steps can be composed directly. Once an async step appears, the resulting chain becomes async.

Sync followed by async:

```csharp
IExecutable<string, int> parse = Executable.Create((string text) => int.Parse(text));

IAsyncExecutable<string, string> mixed =
  parse.Then(async (int value, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {value}";
  });
```

Async followed by sync:

```csharp
IAsyncExecutable<int, string> formatAsync =
  AsyncExecutable.Create(async (int value, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {value}";
  });

IAsyncExecutable<string, string> mixedReverse =
  formatAsync.Compose((string text) => int.Parse(text));
```

The same interop applies to the surrounding API:

- queries can be promoted to async queries,
- commands can be combined across sync/async boundaries through `Append(...)` and `Prepend(...)`,
- runtime execution can continue on `IAsyncExecutor<TIn, TOut>` with async policies and async middleware.

## Async Runtime

Async executors keep the same runtime model as sync executors:

- `WithPolicy(...)`
- `WithContext(...)`
- `WithResult()`
- `SuppressException().OfType<TEx>()`
- `MapException(...)`
- `Tap(...)`
- partial execution through `Execute(...)`

The main async-specific additions are:

- cancellation tokens,
- timeout and retry policies,
- race-based execution,
- `CancelAfterCompletion()` for linked async work.

```csharp
IAsyncExecutor<string, Result<int>> runtime =
  AsyncExecutable.Create(async (string text, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return int.Parse(text);
  })
  .GetExecutor()
  .WithPolicy(builder => builder
    .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
    .Timeout(TimeSpan.FromSeconds(1)))
  .WithResult();
```

## Partial Execution

Tuple-shaped async executors support the same partial execution model as sync executors.

```csharp
IAsyncExecutor<(int, int, int, int), string> format =
  AsyncExecutable.Create(async (int x, int y, int z, int w, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return $"x: {x}, y: {y}, z: {z}, w: {w}";
  })
  .GetExecutor();

string value = await format.Execute(1).Execute(10).Execute(50).Execute(100);
```

This is the same currying model as sync executors, just with `ValueTask<T>` at the final execution point.

## Choosing the Right Style

Use sync when the work is immediate and local. Use async when execution may wait on I/O, timers, external resources, or
when cancellation needs to flow through the pipeline.

Because the models stay aligned, switching to async does not require learning a different composition style.
