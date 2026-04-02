# Composition

## Chaining with `Then(...)`

`Then(...)` is the basic pipeline operator: output of one executable becomes input of the next.

```csharp
IExecutable<string, int> parseLength =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.Length);
```

## Branching with `Fork(...)`

`Fork(...)` sends one result into two branches and returns a tuple.

```csharp
IExecutable<string, (int, string)> info =
  Executable.Create((string text) => text.Trim())
    .Fork(
      text => text.Length,
      text => text.ToUpperInvariant());
```

## Tuple Helpers

After a fork, tuple-oriented operators help transform or merge branches:

- `First(...)`
- `Second(...)`
- `Swap()`
- `Merge(...)`

```csharp
IExecutable<string, string> summary =
  Executable.Create((string text) => text.Trim())
    .Fork(text => text.Length, text => text.ToUpperInvariant())
    .Merge((length, upper) => $"{upper} ({length})");
```

## Contract Adaptation

`Map(...)`, `InMap(...)`, and `OutMap(...)` adapt an executable to a different external contract.

```csharp
IExecutable<int, int> square = Executable.Create((int x) => x * x);

IExecutable<string, string> squareText =
  square.Map(
    text => int.Parse(text),
    value => $"Result: {value}");
```

## Reusable Transformations

`Tap(...)` observes results while preserving them, and `Pipe(...)` applies a reusable composition function.

```csharp
IExecutable<int, string> pipeline =
  Executable.Create((int x) => x + 1)
    .Tap(value => Console.WriteLine(value))
    .Pipe(executable => executable.Then(value => $"Value: {value}"));
```

## Query and Command Composition

Queries compose with `Connect(...)`, commands compose with `Compose(...)`.

```csharp
IQuery<string, int> parse =
  Executable.Create((string text) => int.Parse(text))
    .AsQuery();

IQuery<int, string> format =
  Executable.Create((int value) => $"Value: {value}")
    .AsQuery();

IQuery<string, string> chained = parse.Connect(format);
```

```csharp
ICommand<string> first =
  Executable.Create((string value) => true).AsCommand();

ICommand<string> second =
  Executable.Create((string value) => true).AsCommand();

ICommand<string> combined = first.Compose(second);
```

`Compose(...)` uses short-circuit semantics: if the first command returns `false`, the second command is not executed.

## Racing Async Executables

For asynchronous pipelines, `Race(...)` and `RaceSuccess(...)` let multiple follow-up executables compete against each
other.

- `Race(...)` returns the first completed result,
- `RaceSuccess(...)` returns the first successful result.
- `Race(...)` fails when the first completed executable finishes with an exception.
- `RaceSuccess(...)` fails only when all competing executables fail:
  - if all of them fail with exceptions, those exceptions are aggregated,
  - if all of them are canceled, it throws `OperationCanceledException`.

These operators are useful when several asynchronous providers can handle the same input and you want either:

- the fastest answer, or
- the fastest successful answer.

```csharp
IAsyncExecutable<string, int> parse =
  AsyncExecutable.Create(async (string text, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return int.Parse(text);
  });

IAsyncExecutable<string, string> fastest =
  parse.Race(
    async (int value, CancellationToken token) =>
    {
      await Task.Delay(50, token);
      return $"Slow: {value}";
    },
    async (int value, CancellationToken token) =>
    {
      await Task.Delay(5, token);
      return $"Fast: {value}";
    });

string fastestResult = await fastest.GetExecutor().Execute("42");
// Returns "Fast: 42".
```

```csharp
IAsyncExecutable<string, string> firstSuccessful =
  parse.RaceSuccess(
    async (int value, CancellationToken token) =>
    {
      await Task.Delay(5, token);
      throw new InvalidOperationException("Provider failed");
    },
    async (int value, CancellationToken token) =>
    {
      await Task.Delay(20, token);
      return $"Recovered: {value}";
    });

string recoveredResult = await firstSuccessful.GetExecutor().Execute("42");
// Returns "Recovered: 42".
```

If you want the losing executions to be canceled after the first completion, combine racing with
`CancelAfterCompletion()`.

```csharp
bool canceled = false;

IAsyncExecutable<string, string> fastestWithCancellation =
  AsyncExecutable.Create(async (string text, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return text;
  })
  .Race(
    async (string value, CancellationToken token) =>
    {
      try
      {
        await Task.Delay(100, token);
      }
      catch (OperationCanceledException)
      {
        canceled = true;
      }

      return $"Slow: {value}";
    },
    async (string value, CancellationToken token) =>
    {
      await Task.Delay(5, token);
      return $"Fast: {value}";
    })
  .WithPolicy(policy => policy.CancelAfterCompletion());

string result = await fastestWithCancellation.GetExecutor().Execute("42");
// Returns "Fast: 42", and `canceled` becomes true for the losing execution.
```

## Explicit Branching

For condition-based routing, use `Branch` and `AsyncBranch`.

```csharp
int state = 1;

IQuery<Unit, string> stateText = Branch<string>
  .If(() => state == 0, () => "Init")
  .ElseIf(() => state == 1, () => "Running")
  .Else(() => "Unknown")
  .AsQuery();
```
