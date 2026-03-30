# Overview

`Executables` is a composable .NET library for modeling application behavior as reusable executable units.

Instead of centering application logic around framework-specific services, handlers, or ad hoc delegate chains, the
library gives a small set of abstractions for:

- describing work,
- composing work,
- executing work,
- decorating work with cross-cutting behavior.

At the center of the model is the idea that commands, queries, event publishing, mapping, validation, branching, and
policy-based execution can all be represented as executable pipelines with explicit input and output contracts.

## What Problems It Solves

The library is most useful when business logic starts spreading across UI callbacks, service methods, middleware,
validators, retry wrappers, and event handlers, making execution flow harder to reuse and reason about.

It is especially useful when you need to:

- wrap delegates into strongly typed reusable operations,
- compose logic from small steps,
- adapt the same logic to different contracts,
- apply guards, validation, retry, timeout, fallback, or context consistently,
- expose the same logic as commands, queries, or events.

## Design Goals

The API is built around a few core principles:

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

The library is probably unnecessary when the application only needs straightforward direct method calls and does not
benefit from composition, reuse, or cross-cutting execution rules.

It is also not intended to be:

- a DI container,
- a distributed messaging system,
- a reactive stream library,
- a full opinionated application architecture.
