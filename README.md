# Executables

Build application logic once - run it with different runtime behavior.

`Executables` is a composable .NET library for modeling application behavior as reusable, strongly typed operations.

It separates:

- what your code does: executable composition,
- how your code runs: runtime policies, branching, context, error handling, and execution control.

That means the same logic can be reused across different application boundaries and executed with different runtime
rules without rewriting the core flow.

## Why Executables

In real applications, behavior often gets scattered across many places:

- business logic in services,
- validation in validators,
- retries and timeouts in wrappers,
- error handling in middleware,
- branching logic inside large methods,
- commands, queries, and event handlers as separate API shapes.

Over time, that makes execution flow harder to reuse, test, adapt, and reason about.

`Executables` gives you one explicit composition model for that behavior. It becomes useful when direct method calls
are no longer enough because the same logic needs to be:

- composable,
- reusable across multiple entry points,
- decorated with runtime rules,
- strongly typed from input to output,
- easy to test in isolation.

Typical fits include:

- application-layer orchestration,
- validation-heavy workflows,
- query and command pipelines,
- explicit branching and fallback flows,
- policy-based execution with retry, timeout, guards, and context propagation.

With it, you can define logic once, compose it from small typed steps, expose it as a query, command, event
subscriber, or handler, and decide later how it should execute at runtime.

## The Core Idea

The library revolves around one main split:

- `IExecutable<TIn, TOut>` describes behavior as a reusable typed value,
- `IExecutor<TIn, TOut>` runs that behavior with runtime decoration and execution control.

That split is the main reason the library exists. It lets you keep composition stable while changing runtime behavior.

The same executable chain can be executed with validation, fallback, timeout, retry, exception mapping, context
initialization, result wrapping, caching, metrics, or middleware pipelines without changing the original composition.

> Define behavior once. Run it many ways.

## Quick Example

```csharp
IExecutor<string, Result<int>> parseLength =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.Length)
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .ValidateOutput(length => length >= 0, "Length must be non-negative"))
    .WithResult();

Result<int> result = parseLength.Execute("  demo  ");
```

What happens here:

- `Executable.Create(...)` wraps behavior as a reusable executable unit.
- `Then(...)` composes the next step.
- `GetExecutor()` moves from composition to execution.
- `WithPolicy(...)` adds runtime validation.
- `WithResult()` turns success and failure into an explicit return contract.

The important part is that the executable composition itself stays reusable.

## Reusing The Same Composition

```csharp
IExecutable<string, int> parse =
  Executable.Create((string text) => text.Trim())
    .Then(text => int.Parse(text));

IExecutor<string, Result<int>> safe =
  parse
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .Fallback<FormatException>((_, _) => 0))
    .WithResult();

IExecutor<string, Optional<int>> forgiving =
  parse
    .GetExecutor()
    .SuppressException()
    .OfType<FormatException>();
```

This is the same composition executed through two different runtime contracts:

- `safe` returns `Result<int>`,
- `forgiving` returns `Optional<int>`.

The logic is the same. Only the runtime changes.

## Main Building Blocks

`Executables` gives you one model that covers reusable executable units, pipeline composition, explicit branching,
commands and queries, event-driven flows, executor-level policies, middleware-style runtime pipelines, and aligned sync
and async APIs.

### Executable Composition

Use `Executable.Create(...)`, `Then(...)`, `Compose(...)`, `Fork(...)`, `Merge(...)`, `FlatMap(...)`, `Map(...)`, and
related operators to build reusable behavior.

```csharp
IExecutable<string, string> pipeline =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.ToUpperInvariant())
    .Then(text => $"Value: {text}");
```

### Runtime Execution and Policies

Convert a composition into an executor and decorate it with runtime behavior such as validation, guards, fallback,
timeout, retry, reentrancy prevention, or completion-driven cancellation.

```csharp
IAsyncExecutor<string, Result<int>> runtime =
  AsyncExecutable.Create(async (string text, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return int.Parse(text);
  })
  .GetExecutor()
  .WithPolicy(policy => policy
    .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
    .Timeout(TimeSpan.FromSeconds(1)))
  .WithResult();
```

### Queries, Commands, and Events

Expose executable logic through more specific application-oriented contracts when you want clearer intent at the API
boundary.

```csharp
IQuery<string, string> query = pipeline.AsQuery();

ICommand<string> command =
  Executable.Create((string text) => !string.IsNullOrWhiteSpace(text))
    .AsCommand();

var changed = new Event<string>();
changed.Subscribe(value => Console.WriteLine($"Changed: {value}"));
changed.Handle(EventPublisher.Sequential<string>());
changed.Publish("Updated");
```

### Middleware Pipelines

Build runtime execution through middleware-style chains when that shape is more natural than isolated policies.

```csharp
IExecutor<string, string> executor = Pipeline
  .Use((string text, Func<string, int> next) =>
  {
    string normalized = text.Trim();
    int length = next(normalized);
    return $"Length: {length}";
  })
  .End(Executable.Create((string text) => text.Length));
```

### Sync and Async APIs

The library keeps synchronous and asynchronous APIs structurally parallel, so moving to async does not require
learning a different model.

```csharp
IExecutable<string, int> parse = Executable.Create((string text) => int.Parse(text));

IAsyncExecutable<string, string> asyncPipeline =
  parse.Then(async (int value, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {value}";
  });
```

## Why Not Just Use Methods, Delegates, or Middleware?

Direct methods are often enough for small, local, straightforward code. `Executables` becomes useful when behavior
needs to be extracted from ad hoc call chains, shared across different application boundaries, composed from reusable
steps, executed with different runtime policies, or adapted without rewriting the core logic.

Delegates are a good low-level representation of behavior, but they do not by themselves give you a consistent
composition API, reusable runtime decoration, specialized contracts like queries and commands, structured branching,
policy pipelines, reversible typed adaptation, or aligned sync and async executable models.

Middleware and resilience libraries solve important parts of the problem, but usually from specific angles.
`Executables` models application behavior itself as typed composable values and then lets you apply runtime control
around that behavior. Depending on the use case, these approaches can complement each other.

## When to Use It

`Executables` fits especially well when application behavior should be:

- explicit,
- strongly typed,
- reusable,
- testable in isolation,
- adaptable to multiple runtime environments,
- composed without committing to a heavy framework.

Good fits include:

- application services,
- orchestration layers,
- command and query flows,
- reusable workflow steps,
- boundary-heavy code with validation and fallback logic,
- explicit runtime control around execution.

## When Not to Use It

`Executables` is probably not the right tool when:

- direct methods already express behavior clearly,
- the application is very small and simple,
- composition is not important,
- runtime decoration is minimal,
- introducing an explicit abstraction layer would add more complexity than value.

It is also not:

- a DI container,
- a distributed messaging system,
- a reactive stream library,
- a framework that dictates your entire architecture.

## Installation

```powershell
dotnet add package Executables
```

Supported target frameworks:

- `net9.0`
- `netstandard2.1`
- `net48`

## Documentation

- [Manual](https://dducode.github.io/executables/manual/introduction.html)
- [Getting Started](https://dducode.github.io/executables/manual/getting-started.html)
- [Conceptual Model](https://dducode.github.io/executables/manual/conceptual-model.html)
- [Composition](https://dducode.github.io/executables/manual/composition.html)
- [Execution Runtime](https://dducode.github.io/executables/manual/execution-runtime.html)

## Local Documentation

The repository uses a local `DocFX` tool manifest.

```powershell
dotnet tool restore
dotnet docfx docs\docfx.json --serve
```

## Project Files

- [Changes](CHANGES.md)
- [License](LICENSE.md)