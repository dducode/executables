# Conceptual Model

## Executables as the Core Abstraction

The central idea behind `Executables` is that application behavior can be represented as values with an explicit input
and output contract.

Instead of treating logic as "just a method" or "just a handler", the library treats it as an executable object that
can be reused, transformed, decorated, and composed.

## Separating Definition from Invocation

An `IExecutable<TIn, TOut>` describes an operation. An `IExecutor<TIn, TOut>` is the runtime object that actually runs
`Execute(...)`.

This split keeps the model flexible:

- executables are reusable, pure composition building blocks,
- executors are the runtime invocation boundary where policies and execution control are applied.

## Commands, Queries, and Events

Commands, queries, and events are treated as related interaction contracts, not as isolated patterns:

- commands represent action-oriented execution,
- queries represent request/response execution,
- events represent publish/subscribe execution.

Because they sit close to the same abstraction family, the same core logic can often be exposed through more than one
shape.

## Composition Over Inheritance

The library prefers combining operations over building service hierarchies.

Typical flow:

1. create a small executable,
2. compose it with other steps,
3. adapt contracts where necessary,
4. get an executor and decorate runtime behavior,
5. expose it as a query, command, or event-facing API.

## Sync and Async as Parallel APIs

Synchronous and asynchronous pipelines are modeled as parallel APIs with matching mental models.

The distinction stays explicit:

- sync chains stay simple,
- async chains stay strongly typed,
- mixed sync/async flows are supported through explicit adaptation.
