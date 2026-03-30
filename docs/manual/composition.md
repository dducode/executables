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
