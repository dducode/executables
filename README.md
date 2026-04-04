# Executables

Composable .NET primitives for building executable application logic from reusable, strongly typed operations.

## Why Executables

`Executables` gives one consistent model for:

- reusable executable units,
- pipeline composition,
- commands, queries, and events,
- execution operators and policies,
- parallel sync and async APIs.

It is useful when business logic starts spreading across services, UI callbacks, middleware, validators, handlers, and
retry wrappers, and you want one explicit composition model instead of many ad hoc ones.

## Quick Example

```csharp
IQuery<string, Result<int>> parseLength =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.Length)
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .ValidateOutput(length => length >= 0, "Length must be non-negative"))
    .WithResult()
    .AsQuery();

Result<int> result = parseLength.Send("  demo  ");
```

## Core Ideas

The library is easiest to understand as a few related conceptual areas:

1. `IExecutable<TIn, TOut>` and `IExecutor<TIn, TOut>` as the executable core.
2. `IQuery` and `ICommand` as specialized executable contracts.
3. `IEvent` and subscribers as the publication/subscription model.
4. `IHandleable` and handlers as optional attachment-oriented abstractions.
5. operators and policies as reusable execution decorators.

Typical flow:

1. create an executable from a delegate,
2. compose it with other steps,
3. adapt or decorate it,
4. expose it as a query, command, event-driven, or attachment-based API.

## When to Use It

`Executables` fits well when application behavior should be explicit, strongly typed, composable, and reusable across
multiple entry points.

It is especially useful for:

- application-layer orchestration,
- validation-heavy flows,
- reusable query and command logic,
- event-driven notification flows,
- cross-cutting execution rules such as guards, retry, fallback, timeout, and context propagation.

It is probably unnecessary if the codebase only needs straightforward direct method calls and does not benefit from
composition or reusable execution rules.

## Documentation

- [Manual](docs/manual/introduction.md)
- [Getting Started](docs/manual/getting-started.md)

## Installation

```powershell
dotnet add package Executables
```

Supported target frameworks:

- `net9.0`
- `netstandard2.1`
- `net48`

## Local Documentation

The repository uses a local `DocFX` tool manifest.

```powershell
dotnet tool restore
dotnet docfx docs\docfx.json --serve
```

## Project Files

- [Changes](CHANGES.md)
- [License](LICENSE.md)
