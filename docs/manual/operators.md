# Operators

## Operator Model

Operators wrap executors and define runtime behavior around `Execute(...)`.

Use operators when you need runtime concerns:

- error mapping and suppression,
- context initialization,
- metrics and caching,
- threading/scheduling,
- cross-cutting logging/tracing.

Keep business flow composition in `IExecutable` (`Then`, `Compose`, `When/OrWhen/OrElse`), and move runtime behavior to
`IExecutor`.

The core abstractions are:

- `ExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`
- `BehaviorOperator<TIn, TOut>`
- `AsyncExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`
- `AsyncBehaviorOperator<TIn, TOut>`

`BehaviorOperator` preserves the contract. `ExecutionOperator` can adapt it.

## Applying Operators

Attach operators with `Apply(...)`.

```csharp
IExecutor<string, string> executor =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .Apply(ExecutionOperator.Create((string text, IExecutor<string, int> next) =>
    {
      int value = next.Execute(text);
      return $"Parsed: {value}";
    }));
```

Order matters: each `Apply(...)` wraps the previous executor.

```csharp
IExecutor<string, string> pipeline =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .MapException((FormatException ex) => new InvalidOperationException("Invalid number", ex))
    .Tap(value => Console.WriteLine($"Parsed: {value}"))
    .Apply(ExecutionOperator.Create((string text, IExecutor<string, int> next) => $"Value: {next.Execute(text)}"));
```

## Built-in High-Level Operators

Common executor-level runtime helpers include:

- `WithContext(...)`
- `WithResult()`
- `SuppressException().OfType<TEx>()`
- `OnThreadPool()`
- `Cache(...)`
- `Metrics(...)`

```csharp
IExecutor<string, Optional<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .SuppressException()
    .OfType<FormatException>();
```

Another common chain is result-based error capture:

```csharp
IExecutor<string, Result<int>> safeParse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .MapException((FormatException ex) => new InvalidOperationException("Invalid number format", ex))
    .WithResult();
```

## Operator Factories

For lower-level control, use:

- `ExecutionOperator.Create(...)`
- `AsyncExecutionOperator.Create(...)`

Helpers such as `WithContext(...)`, `WithResult()`, `SuppressException(...)`, `Cache(...)`, `Metrics(...)`,
`MapException(...)`, and `OnThreadPool()` are exposed as executor extensions.

```csharp
ExecutionOperator<string, int, int, string> formatSquared =
  ExecutionOperator.Create((string text, IExecutor<int, int> next) =>
  {
    int value = int.Parse(text);
    int squared = next.Execute(value);
    return $"Squared: {squared}";
  });
```

Async operator factory example:

```csharp
IAsyncExecutor<string, string> asyncExecutor =
  AsyncExecutable.Create((string input, CancellationToken _) => ValueTask.FromResult(input.Trim()))
    .GetExecutor()
    .Apply(AsyncExecutionOperator.Create(async (string input, IAsyncExecutor<string, string> next, CancellationToken token) =>
    {
      string result = await next.Execute(input, token);
      return result.ToUpperInvariant();
    }));
```

## Pipelines and Middlewares

`Pipeline` and `AsyncPipeline` are middleware builders built on the operator layer.

Each middleware receives:

- the current input,
- a typed `next` delegate,
- and, for async chains, a cancellation token.

The main limitation is that a single chain cannot mix sync and async middleware.

```csharp
IExecutor<string, string> executor = Pipeline
  .Use((string text, Func<int, int> next) =>
  {
    int length = next(text.Trim().Length);
    return $"Length: {length}";
  })
  .End(Executable.Create((int length) => length * 2));
```

Async pipeline example:

```csharp
IAsyncExecutor<string, string> executor = AsyncPipeline
  .Use(async (string text, AsyncFunc<int, int> next, CancellationToken token) =>
  {
    int length = text.Trim().Length;
    int doubled = await next.Invoke(length, token);
    return $"Length: {doubled}";
  })
  .End(AsyncExecutable.Create((int length, CancellationToken _) => ValueTask.FromResult(length * 2)));
```

## Writing Custom Operators

When built-in operators are not enough, create an operator type directly.

```csharp
public sealed class TrimBehaviorOperator : BehaviorOperator<string, string> {
  public override string Invoke(string input, IExecutor<string, string> executor) {
    return executor.Execute(input.Trim());
  }
}

IExecutor<string, string> trimmed =
  Executable.Create((string value) => value)
    .GetExecutor()
    .Apply(new TrimBehaviorOperator());
```
