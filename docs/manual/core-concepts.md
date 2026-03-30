# Core Concepts

## Three Layers

The library can be understood as three connected layers:

1. executables and executors as the core execution model,
2. handleables and interaction contracts on top of that model,
3. operators and policies that decorate execution.

## Executables and Executors

`IExecutable<TIn, TOut>` represents behavior as a reusable typed value. `IExecutor<TIn, TOut>` is the object that
actually performs `Execute(...)`.

This split matters because most composition happens at the executable level, while execution happens at the executor
level.

## Handleables

The interaction layer provides:

- `ICommand<T>` / `IAsyncCommand<T>`,
- `IQuery<TIn, TOut>` / `IAsyncQuery<TIn, TOut>`,
- `IEvent<T>`.

Under the hood, base implementations are handleables. The core attachment model is:

- `IHandleable<TIn, TOut, THandler>`,
- `IAsyncHandleable<TIn, TOut, THandler>`.

This means the contract and the runtime behavior can be separated: the interaction object owns the API shape, while a
handler provides the actual execution logic.

## Operators and Policies

Operators wrap execution around an executor.

They can either:

- preserve the contract, via `BehaviorOperator` / `AsyncBehaviorOperator`,
- adapt the contract, via `ExecutionOperator` / `AsyncExecutionOperator`.

Policies are a specialized operator family focused on execution rules such as validation, guards, retry, fallback,
timeout, and reentrancy control.

## Result Shapes

Three common result shapes appear often in the library:

- `Unit` for "no meaningful value",
- `Optional<T>` for absence/presence flows,
- `Result<T>` for success/failure flows with preserved exception information.
