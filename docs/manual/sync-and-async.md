# Sync and Async

## Parallel APIs

`IExecutable<TIn, TOut>` and `IAsyncExecutable<TIn, TOut>` are parallel contracts with matching mental models.

- sync path returns `TOut`
- async path returns `ValueTask<TOut>`

## Converting Sync to Async

`ToAsyncExecutable()` adapts a sync executable to an async one.

```csharp
IExecutable<int, int> square = Executable.Create((int x) => x * x);
IAsyncExecutable<int, int> squareAsync = square.ToAsyncExecutable();
```

The same idea applies to queries and commands through `ToAsyncQuery()` and `ToAsyncCommand()`.

## Mixed Composition

Mixed sync/async chains are supported directly:

- sync executable followed by async executable,
- async executable followed by sync executable,
- `Then(...)` and `Compose(...)` across sync/async boundaries,
- mixed query composition with `Connect(...)`,
- mixed command composition with `Compose(...)`.

```csharp
IExecutable<string, int> parse = Executable.Create((string text) => int.Parse(text));

IAsyncExecutable<string, string> mixed =
  parse.Then(async (int x, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {x}";
  });
```

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

## Partial Execution (Currying)

For tuple-input executors, `Execute(...)` supports partial application.

```csharp
IExecutor<(int, int), int> sum =
  Executable.Create((int x, int y) => x + y)
    .GetExecutor();

IExecutor<int, int> addFive = sum.Execute(5);
int result = addFive.Execute(3); // 8
```

The same pattern works for larger arities:

```csharp
IExecutor<(int, int, int, int), string> format =
  Executable.Create((int x, int y, int z, int w) => $"x: {x}, y: {y}, z: {z}, w: {w}")
    .GetExecutor();

string value = format.Execute(1).Execute(10).Execute(50).Execute(100);
```

Async executors provide the same partial-execution shape:

```csharp
IAsyncExecutor<(int, int, int, int), string> formatAsyncExecutor =
  AsyncExecutable.Create(async (int x, int y, int z, int w, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return $"x: {x}, y: {y}, z: {z}, w: {w}";
  })
  .GetExecutor();

string asyncValue = await formatAsyncExecutor.Execute(1).Execute(10).Execute(50).Execute(100);
```

## Choosing the Right Style

Use sync by default for fast local work. Use async when the operation can block on external resources or needs
cancellation support.
