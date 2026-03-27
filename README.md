# interactions

<details>
<summary>Table of contents</summary>

- [1. Overview](#1-overview)
    - [1.1. What the Interactions Is](#11-what-the-interactions-is)
    - [1.2. What Problems It Helps Solve](#12-what-problems-it-helps-solve)
    - [1.3. Design Goals](#13-design-goals)
    - [1.4. When to Use It](#14-when-to-use-it)
    - [1.5. When Not to Use It](#15-when-not-to-use-it)
- [2. Conceptual Model](#2-conceptual-model)
    - [2.1. Executables as the Core Abstraction](#21-executables-as-the-core-abstraction)
    - [2.2. Separating Execution from Invocation](#22-separating-execution-from-invocation)
    - [2.3. Commands, Queries, and Events](#23-commands-queries-and-events)
    - [2.4. Composition Over Inheritance](#24-composition-over-inheritance)
    - [2.5. Sync and Async as Parallel APIs](#25-sync-and-async-as-parallel-apis)
- [3. Core Concepts for Users](#3-core-concepts-for-users)
    - [3.1. `IExecutable<TIn, TOut>`](#31-iexecutabletin-tout)
    - [3.2. `IExecutor<TIn, TOut>`](#32-iexecutortin-tout)
    - [3.3. `ICommand<T>`](#33-icommandt)
    - [3.4. `IQuery<TIn, TOut>`](#34-iquerytin-tout)
    - [3.5. `IEvent<T>`](#35-ieventt)
    - [3.6. Handlers and Subscribers](#36-handlers-and-subscribers)
    - [3.7. `Unit`, `Optional<T>`, and `Result<T>`](#37-unit-optionalt-and-resultt)
    - [3.8. Interaction Context](#38-interaction-context)
    - [3.9. History, Undo, and Redo](#39-history-undo-and-redo)
- [4. Creating API Objects](#4-creating-api-objects)
    - [4.1. Creating Executables with `Executable.Create(...)`](#41-creating-executables-with-executablecreate)
    - [4.2. Creating Async Executables with
      `AsyncExecutable.Create(...)`](#42-creating-async-executables-with-asyncexecutablecreate)
    - [4.3. Identity Executables](#43-identity-executables)
    - [4.4. Converting Executables to Commands](#44-converting-executables-to-commands)
    - [4.5. Converting Executables to Queries](#45-converting-executables-to-queries)
    - [4.6. Converting Executables to Handlers](#46-converting-executables-to-handlers)
    - [4.7. Creating and Using Events](#47-creating-and-using-events)
- [5. Composition API](#5-composition-api)
    - [5.1. Chaining with `Then(...)`](#51-chaining-with-then)
    - [5.2. Branching with `Fork(...)`](#52-branching-with-fork)
    - [5.3. Transforming Tuple Outputs with `First(...)`, `Second(...)`, and
      `Swap()`](#53-transforming-tuple-outputs-with-first-second-and-swap)
    - [5.4. Combining Branches with `Merge(...)`](#54-combining-branches-with-merge)
    - [5.5. Adapting Contracts with `Map(...)`, `InMap(...)`, and
      `OutMap(...)`](#55-adapting-contracts-with-map-inmap-and-outmap)
    - [5.6. Observing Results with `Tap(...)`](#56-observing-results-with-tap)
    - [5.7. Building Reusable Pipelines with `Pipe(...)`](#57-building-reusable-pipelines-with-pipe)
- [6. Commands, Queries, and Events](#6-commands-queries-and-events)
    - [6.1. Command Semantics](#61-command-semantics)
    - [6.2. Query Semantics](#62-query-semantics)
    - [6.3. Event Publishing](#63-event-publishing)
    - [6.4. Subscribing and Unsubscribing](#64-subscribing-and-unsubscribing)
    - [6.5. One-Time Subscriptions](#65-one-time-subscriptions)
    - [6.6. Parameterless Operations](#66-parameterless-operations)
- [7. Policies and Execution Control](#7-policies-and-execution-control)
    - [7.1. Applying Policies with `WithPolicy(...)`](#71-applying-policies-with-withpolicy)
    - [7.2. Input and Output Validation](#72-input-and-output-validation)
    - [7.3. Guard Conditions](#73-guard-conditions)
    - [7.4. Retry, Timeout, and Fallback Patterns](#74-retry-timeout-and-fallback-patterns)
    - [7.5. Preventing Reentrancy](#75-preventing-reentrancy)
    - [7.6. Running Work on the Thread Pool](#76-running-work-on-the-thread-pool)
- [8. Context, Safety, and Error Handling](#8-context-safety-and-error-handling)
    - [8.1. Running with `WithContext(...)`](#81-running-with-withcontext)
    - [8.2. Correlation and Nested Execution Contexts](#82-correlation-and-nested-execution-contexts)
    - [8.3. Wrapping Execution with `WithResult()`](#83-wrapping-execution-with-withresult)
    - [8.4. Suppressing Exceptions](#84-suppressing-exceptions)
    - [8.5. Optional-Based Error Handling](#85-optional-based-error-handling)
    - [8.6. Disposal and Lifetime Considerations](#86-disposal-and-lifetime-considerations)
- [9. Synchronous and Asynchronous Usage](#9-synchronous-and-asynchronous-usage)
    - [9.1. `IExecutable` vs `IAsyncExecutable`](#91-iexecutable-vs-iasyncexecutable)
    - [9.2. `ToAsyncExecutable()`](#92-toasyncexecutable)
    - [9.3. `ToAsyncCommand()` and `ToAsyncQuery()`](#93-toasynccommand-and-toasyncquery)
    - [9.4. Mixing Sync and Async Chains](#94-mixing-sync-and-async-chains)
    - [9.5. Choosing the Right API Style](#95-choosing-the-right-api-style)
- [10. Common Usage Patterns](#10-common-usage-patterns)
    - [10.1. Building a Simple Processing Pipeline](#101-building-a-simple-processing-pipeline)
    - [10.2. Wrapping Existing Delegates into the Library Model](#102-wrapping-existing-delegates-into-the-library-model)
    - [10.3. Adding Cross-Cutting Rules Around Business Logic](#103-adding-cross-cutting-rules-around-business-logic)
    - [10.4. Using Events for Notification Flows](#104-using-events-for-notification-flows)
    - [10.5. Implementing Undo/Redo Behavior](#105-implementing-undoredo-behavior)
    - [10.6. Integrating with UI Actions or Application Services](#106-integrating-with-ui-actions-or-application-services)
- [11. API Quick Reference](#11-api-quick-reference)
    - [11.1. Static Entry Points](#111-static-entry-points)
    - [11.2. Most Important Extension Methods](#112-most-important-extension-methods)
    - [11.3. Most Important Return Types](#113-most-important-return-types)
    - [11.4. Recommended Starting Points for New Users](#114-recommended-starting-points-for-new-users)
- [12. Practical Notes](#12-practical-notes)
    - [12.1. Target Frameworks](#121-target-frameworks)
    - [12.2. Package Usage Expectations](#122-package-usage-expectations)
    - [12.3. Thread-Safety Notes](#123-thread-safety-notes)
    - [12.4. Performance and Allocation Considerations](#124-performance-and-allocation-considerations)
    - [12.5. Limitations and Tradeoffs](#125-limitations-and-tradeoffs)
- [13. Suggested Expansion Order](#13-suggested-expansion-order)
    - [13.1. Overview and Core Concepts First](#131-overview-and-core-concepts-first)
    - [13.2. Creation and Composition API Next](#132-creation-and-composition-api-next)
    - [13.3. Policies, Context, and Error Handling After That](#133-policies-context-and-error-handling-after-that)
    - [13.4. Code Examples Where They Matter Most](#134-code-examples-where-they-matter-most)

</details>

## 1. Overview

### 1.1. What the Interactions Is

`interactions` is a composable .NET library for modeling application behavior as reusable executable units. Instead of
centering the design around framework-specific handlers, service classes, or ad hoc delegate chains, it gives you a
small set of abstractions for describing work, executing it, composing it, and decorating it with cross-cutting rules.

At the center of the library is the idea that many kinds of application logic can be treated uniformly: commands,
queries, event publishing, synchronous steps, asynchronous steps, validation, branching, and policy-based execution all
become variations of the same executable pipeline model.

### 1.2. What Problems It Helps Solve

The library is useful when business logic starts spreading across UI callbacks, service methods, middleware, validators,
retry wrappers, and event handlers, making execution flow harder to understand and reuse. It provides a common way to
package logic into explicit units and combine them without rewriting infrastructure around each use case.

It is especially helpful when you need to:

- turn delegates into strongly-typed reusable operations,
- build pipelines from small steps,
- adapt the same logic to different input and output contracts,
- apply validation, guards, fallback, retry, or timeout behavior consistently,
- expose the same logic as commands, queries, or event-driven flows,
- keep sync and async variants conceptually aligned.

### 1.3. Design Goals

The design favors a few specific goals:

- explicit execution model instead of implicit call chains,
- composition-first APIs instead of deep inheritance or large base classes,
- small, orthogonal abstractions that can be combined in different ways,
- symmetry between synchronous and asynchronous usage,
- low-friction adaptation of existing delegates and application code,
- support for cross-cutting concerns without pushing users into a full framework.

The library aims to stay lightweight at the API level: users should be able to adopt one concept at a time, starting
from simple executable wrappers and moving toward richer pipelines only when needed.

### 1.4. When to Use It

`interactions` is a good fit when you want a clear application-layer execution model without committing to a heavy
architectural framework. Typical use cases include UI action processing, request orchestration, domain operation
pipelines, validation-heavy flows, reusable query logic, event notification chains, and command models with undo/redo
behavior.

It works best in projects where logic benefits from being:

- explicit,
- strongly typed,
- composable,
- testable in isolation,
- reusable across multiple entry points.

### 1.5. When Not to Use It

The library is probably unnecessary if your codebase only needs simple direct method calls and has little composition,
reuse, or cross-cutting execution logic. If the application has very shallow behavior and no meaningful need for
policies, pipeline composition, or sync/async abstraction alignment, the added model may not justify itself.

It may also be the wrong tool if you are specifically looking for:

- a dependency injection container,
- a full mediator framework with built-in ecosystem conventions,
- a distributed messaging system,
- a reactive stream library,
- an opinionated application architecture that dictates project structure.

`interactions` is strongest as a focused execution and composition library, not as a complete platform.

## 2. Conceptual Model

### 2.1. Executables as the Core Abstraction

The central idea behind `interactions` is that application behavior can be represented as executable objects with a
well-defined input and output contract. Instead of treating logic as "just a method" or "just a handler", the library
treats it as a first-class value that can be reused, wrapped, transformed, and composed.

This gives different kinds of behavior a common model. A transformation, a command-like action, a query, a notification
step, or a policy-decorated workflow can all be described as executable logic and combined using the same set of
concepts.

### 2.2. Separating Execution from Invocation

The model separates the definition of an operation from the act of running it. An `IExecutable<TIn, TOut>` describes
something that can provide an executor, while `IExecutor<TIn, TOut>` is the object that actually performs
`Execute(...)`.

This distinction keeps the API flexible. Executables can be composed and adapted as reusable building blocks, while
executors provide a direct execution boundary when the user wants to invoke the final operation explicitly.

### 2.3. Commands, Queries, and Events

`interactions` treats commands, queries, and events as related forms of application behavior rather than isolated
patterns.

- commands represent actions that perform work,
- queries represent operations that return data,
- events represent publish-subscribe notifications.

Because these concepts live near the same abstraction family, the library makes it easier to move between them. A
reusable executable can often be exposed as a command or query, and event-driven flows still fit into the same
execution-oriented model.

### 2.4. Composition Over Inheritance

The library is designed around composition. Instead of encouraging large service hierarchies or framework-specific base
classes, it provides small operations that can be combined with APIs such as `Then(...)`, `Fork(...)`, `Map(...)`,
`Pipe(...)`, `WithPolicy(...)`, and `WithContext(...)`.

This leads to a predictable way of building behavior:

- start with a small executable,
- combine it with other steps,
- adapt inputs and outputs where necessary,
- apply cross-cutting rules around it,
- expose the final result through the API shape the application needs.

That approach keeps behavior explicit and usually makes it easier to test, reason about, and evolve over time.

### 2.5. Sync and Async as Parallel APIs

Another important principle is that synchronous and asynchronous workflows should feel structurally similar. The library
provides sync and async abstractions, along with conversion helpers, so that moving from one style to the other does not
require adopting a completely different mental model.

The APIs stay explicit rather than hiding the distinction:

- synchronous pipelines remain simple,
- asynchronous pipelines remain strongly typed,
- mixed chains are supported through clear adaptation methods.

For users, this means a pipeline can begin as a straightforward synchronous flow and later grow into an asynchronous one
without losing conceptual consistency.

## 3. Core Concepts for Users

### 3.1. `IExecutable<TIn, TOut>`

### 3.2. `IExecutor<TIn, TOut>`

### 3.3. `ICommand<T>`

### 3.4. `IQuery<TIn, TOut>`

### 3.5. `IEvent<T>`

### 3.6. Handlers and Subscribers

### 3.7. `Unit`, `Optional<T>`, and `Result<T>`

### 3.8. Interaction Context

### 3.9. History, Undo, and Redo

## 4. Creating API Objects

### 4.1. Creating Executables with `Executable.Create(...)`

### 4.2. Creating Async Executables with `AsyncExecutable.Create(...)`

### 4.3. Identity Executables

### 4.4. Converting Executables to Commands

### 4.5. Converting Executables to Queries

### 4.6. Converting Executables to Handlers

### 4.7. Creating and Using Events

## 5. Composition API

### 5.1. Chaining with `Then(...)`

### 5.2. Branching with `Fork(...)`

### 5.3. Transforming Tuple Outputs with `First(...)`, `Second(...)`, and `Swap()`

### 5.4. Combining Branches with `Merge(...)`

### 5.5. Adapting Contracts with `Map(...)`, `InMap(...)`, and `OutMap(...)`

### 5.6. Observing Results with `Tap(...)`

### 5.7. Building Reusable Pipelines with `Pipe(...)`

## 6. Commands, Queries, and Events

### 6.1. Command Semantics

### 6.2. Query Semantics

### 6.3. Event Publishing

### 6.4. Subscribing and Unsubscribing

### 6.5. One-Time Subscriptions

### 6.6. Parameterless Operations

## 7. Policies and Execution Control

### 7.1. Applying Policies with `WithPolicy(...)`

### 7.2. Input and Output Validation

### 7.3. Guard Conditions

### 7.4. Retry, Timeout, and Fallback Patterns

### 7.5. Preventing Reentrancy

### 7.6. Running Work on the Thread Pool

## 8. Context, Safety, and Error Handling

### 8.1. Running with `WithContext(...)`

### 8.2. Correlation and Nested Execution Contexts

### 8.3. Wrapping Execution with `WithResult()`

### 8.4. Suppressing Exceptions

### 8.5. Optional-Based Error Handling

### 8.6. Disposal and Lifetime Considerations

## 9. Synchronous and Asynchronous Usage

### 9.1. `IExecutable` vs `IAsyncExecutable`

### 9.2. `ToAsyncExecutable()`

### 9.3. `ToAsyncCommand()` and `ToAsyncQuery()`

### 9.4. Mixing Sync and Async Chains

### 9.5. Choosing the Right API Style

## 10. Common Usage Patterns

### 10.1. Building a Simple Processing Pipeline

### 10.2. Wrapping Existing Delegates into the Library Model

### 10.3. Adding Cross-Cutting Rules Around Business Logic

### 10.4. Using Events for Notification Flows

### 10.5. Implementing Undo/Redo Behavior

### 10.6. Integrating with UI Actions or Application Services

## 11. API Quick Reference

### 11.1. Static Entry Points

### 11.2. Most Important Extension Methods

### 11.3. Most Important Return Types

### 11.4. Recommended Starting Points for New Users

## 12. Practical Notes

### 12.1. Target Frameworks

### 12.2. Package Usage Expectations

### 12.3. Thread-Safety Notes

### 12.4. Performance and Allocation Considerations

### 12.5. Limitations and Tradeoffs

## 13. Suggested Expansion Order

### 13.1. Overview and Core Concepts First

### 13.2. Creation and Composition API Next

### 13.3. Policies, Context, and Error Handling After That

### 13.4. Code Examples Where They Matter Most
