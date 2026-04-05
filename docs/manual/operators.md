# Operators

## Operator Model

Operators are wrappers around executors. They define how execution is transformed before and after the wrapped
executor runs.

The core abstractions are:

- `ExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`
- `BehaviorOperator<TIn, TOut>`
- `AsyncExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`
- `AsyncBehaviorOperator<TIn, TOut>`

`BehaviorOperator` preserves the contract. `ExecutionOperator` can adapt it.

## Applying Operators

Operators are attached with `Apply(...)`.

```csharp
IExecutor<string, string> pipeline =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .MapException((FormatException ex) => new InvalidOperationException("Invalid number", ex))
    .Tap(value => Console.WriteLine($"Parsed: {value}"))
    .Apply(ExecutionOperator.Create((string text, IExecutor<string, int> next) => $"Value: {next.Execute(text)}"));
```

## Built-in High-Level Operators

Common operator-based helpers include:

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

## Operator Factories

For lower-level control, use:

- `ExecutionOperator.Create(...)`
- `ExecutionOperator.Cache(...)`
- `ExecutionOperator.Metrics(...)`
- `ExecutionOperator.Context(...)`
- `ExecutionOperator.MapException(...)`
- async counterparts from `AsyncExecutionOperator`

```csharp
ExecutionOperator<string, int, int, string> formatSquared =
  ExecutionOperator.Create((string text, IExecutor<int, int> next) =>
  {
    int value = int.Parse(text);
    int squared = next.Execute(value);
    return $"Squared: {squared}";
  });
```

## Pipelines and Middlewares

`Pipeline` and `AsyncPipeline` are middleware builders built on the operator layer.

Each middleware receives:

- the current input,
- a `next` delegate,
- and, for async chains, a cancellation token.

The main limitation is that a single chain cannot mix sync and async middleware.

```csharp
IQuery<string, string> query = Pipeline<string, string>
  .Use((string text, Func<int, int> next) =>
  {
    int length = next(text.Trim().Length);
    return $"Length: {length}";
  })
  .End(length => length * 2)
  .AsQuery();
```

## Writing Custom Operators

When built-in operators are not enough, create a custom operator type or build one from a delegate.

```csharp
IExecutor<string, string> trimmed =
  Executable.Create((string value) => value)
    .GetExecutor()
    .Apply(ExecutionOperator.Create((string input, IExecutor<string, string> next) =>
      next.Execute(input.Trim())));
```
