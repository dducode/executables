# Overview

`Executables` provides a small set of abstractions for making application behavior explicit.

The library is built around a few simple capabilities:

- describing work,
- composing work,
- executing work,
- decorating work with cross-cutting behavior.

Commands, queries, event publication, mapping, and branching all fit into that broader model, while validation and
policy-based execution stay at the runtime boundary. Reversible contract adaptation can also be modeled explicitly
through `IIso<T1, T2>`.

## What Problems It Solves

The library is most useful when application behavior starts spreading across UI callbacks, service methods, middleware,
validators, retry wrappers, and event handlers, making execution flow harder to reuse and reason about.

It is especially useful when you need to:

- wrap delegates into strongly typed reusable operations,
- compose logic from small steps,
- adapt the same logic to different contracts,
- apply guards, validation, retry, timeout, fallback, or context consistently,
- expose the same logic as commands, queries, or events.

## Design Goals

The API is built around a few practical principles:

- explicit execution instead of implicit call chains,
- composition over inheritance,
- small orthogonal abstractions,
- symmetry between synchronous and asynchronous usage,
- low-friction adaptation of existing code.

## When to Use It

`Executables` fits well in application-layer code where behavior should be:

- explicit,
- strongly typed,
- testable in isolation,
- reusable across multiple entry points,
- composable without introducing a heavy framework.

## When Not to Use It

The library is a better fit for systems that benefit from explicit composition and reusable execution rules. If the
application is small and straightforward direct method calls already express the behavior clearly, the extra abstraction
may not add much value.

It is also useful to be clear about what this library is not:

- a DI container,
- a distributed messaging system,
- a reactive stream library,
- a framework that dictates the full application architecture.
