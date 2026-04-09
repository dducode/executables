# Core Concepts

## Conceptual Areas

`Executables` is easiest to understand as a small set of related conceptual areas rather than as one flat API surface.

## Executable Core

`IExecutable<TIn, TOut>` represents behavior as a reusable typed value. `IExecutor<TIn, TOut>` is the object that
actually performs `Execute(...)`.

The asynchronous counterparts follow the same idea:

- `IAsyncExecutable<TIn, TOut>`
- `IAsyncExecutor<TIn, TOut>`

This is the foundation for reusable, strongly typed application behavior:

- executable objects define composition and contract shape,
- executor objects define runtime execution and runtime decoration.

Most composition starts on `IExecutable` through operators such as `Then(...)`, `Compose(...)`, `Fork(...)`, and
`Map(...)`.

For reversible transformations between two contracts, the library also provides `IIso<T1, T2>`.

## Executable Contracts

On top of the executable core, the library provides specialized execution contracts for request/response and
action-oriented flows:

- `ICommand<T>` / `IAsyncCommand<T>`,
- `IQuery<TIn, TOut>` / `IAsyncQuery<TIn, TOut>`,
- `IEvent<T>`.

`IQuery` and `ICommand` stay especially close to the executable model. In practice, they are executable-shaped APIs
with more specific intent and usage.

## Events and Subscribers

Events have a slightly different role:

- `IEvent<T>`.
- `Event<T>`
- `ISubscriber<T>`
- event publishers and subscription APIs

Unlike queries and commands, events are not just adapted from executables through a simple `As...` conversion. They
belong to a publication/subscription model with their own runtime semantics.

At the same time, this area still overlaps with the executable model, because executable logic can be adapted into
subscribers and participate in event-driven flows.

## Attachment Model

The library also provides optional attachment-oriented abstractions:

- `IHandleable<TIn, TOut, THandler>`,
- `IAsyncHandleable<TIn, TOut, THandler>`.

This part of the model is useful when API shape and attached behavior should be managed separately, or when behavior
needs to be attached, shared, replaced, or merged explicitly.

## Operators and Policies

Operators and policies wrap execution around an executor.

They can either:

- preserve the contract, via `BehaviorOperator` / `AsyncBehaviorOperator`,
- adapt the contract, via `ExecutionOperator` / `AsyncExecutionOperator`.

Policies are a specialized runtime family focused on validation, guards, retry, fallback, timeout, and reentrancy
control.

## Result Shapes

Three common result shapes appear often in the library:

- `Unit` for "no meaningful value",
- `Optional<T>` for absence/presence flows,
- `Result<T>` for success/failure flows with preserved exception information.
