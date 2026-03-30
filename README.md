# Executables

<details>
<summary>Table of contents</summary>

- [1. Overview](#1-overview)
    - [1.1. What the Executables Is](#11-what-the-executables-is)
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
    - [3.1. Executables and Executors](#31-executables-and-executors)
    - [3.2. Handleables: Query, Command, and Event](#32-handleables-query-command-and-event)
    - [3.3. Operators and Policies](#33-operators-and-policies)
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
    - [5.8. Connecting Queries with `Connect(...)`](#58-connecting-queries-with-connect)
    - [5.9. Composing Commands with `Compose(...)`](#59-composing-commands-with-compose)
    - [5.10. Branching Execution with `Branch` and `AsyncBranch`](#510-branching-execution-with-branch-and-asyncbranch)
- [6. Handleables and Handlers](#6-handleables-and-handlers)
    - [6.1. Missing-Handler Behavior](#61-missing-handler-behavior)
    - [6.2. Subscription Lifetime](#62-subscription-lifetime)
    - [6.3. One-Time Subscriptions](#63-one-time-subscriptions)
    - [6.4. Parameterless Operations](#64-parameterless-operations)
    - [6.5. Handleables and Merging](#65-handleables-and-merging)
    - [6.6. Handler Lifecycle and Disposal](#66-handler-lifecycle-and-disposal)
- [7. Execution Operators](#7-execution-operators)
    - [7.1. Operator Model](#71-operator-model)
    - [7.2. Applying Operators with `Apply(...)`](#72-applying-operators-with-apply)
    - [7.3. Operator Factories: `Create(...)`, `Map(...)`](#73-operator-factories-create-map)
    - [7.4. Context and Result Operators](#74-context-and-result-operators)
    - [7.5. Exception and Scheduling Operators](#75-exception-and-scheduling-operators)
    - [7.6. Cache and Metrics Operators](#76-cache-and-metrics-operators)
    - [7.7. Pipelines and Middlewares](#77-pipelines-and-middlewares)
    - [7.8. Writing Custom Operators](#78-writing-custom-operators)
- [8. Policies and Execution Control](#8-policies-and-execution-control)
    - [8.1. Applying Policies with `WithPolicy(...)`](#81-applying-policies-with-withpolicy)
    - [8.2. Input and Output Validation](#82-input-and-output-validation)
    - [8.3. Guard Conditions](#83-guard-conditions)
    - [8.4. Retry, Timeout, and Fallback Patterns](#84-retry-timeout-and-fallback-patterns)
    - [8.5. Preventing Reentrancy](#85-preventing-reentrancy)
    - [8.6. Validator API: Primitives and Composition](#86-validator-api-primitives-and-composition)
    - [8.7. Guard API: Factory and Composition](#87-guard-api-factory-and-composition)
    - [8.8. Retry Rules API](#88-retry-rules-api)
- [9. Context, Safety, and Error Handling](#9-context-safety-and-error-handling)
    - [9.1. Running with `WithContext(...)`](#91-running-with-withcontext)
    - [9.2. Correlation and Nested Execution Contexts](#92-correlation-and-nested-execution-contexts)
    - [9.3. Suppressing Exceptions with `Optional<T>`](#93-suppressing-exceptions-with-optionalt)
    - [9.4. Wrapping Execution with `WithResult()`](#94-wrapping-execution-with-withresult)
    - [9.5. Disposal and Lifetime Considerations](#95-disposal-and-lifetime-considerations)
- [10. Synchronous and Asynchronous Usage](#10-synchronous-and-asynchronous-usage)
    - [10.1. `IExecutable` vs `IAsyncExecutable`](#101-iexecutable-vs-iasyncexecutable)
    - [10.2. `ToAsyncExecutable()`](#102-toasyncexecutable)
    - [10.3. `ToAsyncCommand()` and `ToAsyncQuery()`](#103-toasynccommand-and-toasyncquery)
    - [10.4. Mixing Sync and Async Chains](#104-mixing-sync-and-async-chains)
    - [10.5. Choosing the Right API Style](#105-choosing-the-right-api-style)
- [11. Common Usage Patterns](#11-common-usage-patterns)
    - [11.1. Building a Simple Processing Pipeline](#111-building-a-simple-processing-pipeline)
    - [11.2. Wrapping Existing Delegates into the Library Model](#112-wrapping-existing-delegates-into-the-library-model)
    - [11.3. Adding Cross-Cutting Rules Around Business Logic](#113-adding-cross-cutting-rules-around-business-logic)
    - [11.4. Using Events for Notification Flows](#114-using-events-for-notification-flows)
    - [11.5. Implementing Undo/Redo Behavior](#115-implementing-undoredo-behavior)
    - [11.6. Integrating with UI Actions or Application Services](#116-integrating-with-ui-actions-or-application-services)
- [12. API Quick Reference](#12-api-quick-reference)
    - [12.1. Static Entry Points](#121-static-entry-points)
    - [12.2. Most Important Extension Methods](#122-most-important-extension-methods)
    - [12.3. Most Important Return Types](#123-most-important-return-types)
    - [12.4. Recommended Starting Points for New Users](#124-recommended-starting-points-for-new-users)
- [13. Practical Notes](#13-practical-notes)
    - [13.1. Target Frameworks](#131-target-frameworks)
    - [13.2. Package Usage Expectations](#132-package-usage-expectations)
    - [13.3. Thread-Safety Notes](#133-thread-safety-notes)
    - [13.4. Performance and Allocation Considerations](#134-performance-and-allocation-considerations)
    - [13.5. Limitations and Tradeoffs](#135-limitations-and-tradeoffs)
- [14. Suggested Expansion Order](#14-suggested-expansion-order)
    - [14.1. Overview and Core Concepts First](#141-overview-and-core-concepts-first)
    - [14.2. Creation and Composition API Next](#142-creation-and-composition-api-next)
    - [14.3. Policies, Context, and Error Handling After That](#143-policies-context-and-error-handling-after-that)
    - [14.4. Code Examples Where They Matter Most](#144-code-examples-where-they-matter-most)

</details>

## 1. Overview

### 1.1. What the Executables Is

`executables` is a composable .NET library for modeling application behavior as reusable executable units. Instead of
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

`executables` is a good fit when you want a clear application-layer execution model without committing to a heavy
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

`executables` is strongest as a focused execution and composition library, not as a complete platform.

## 2. Conceptual Model

### 2.1. Executables as the Core Abstraction

The central idea behind `executables` is that application behavior can be represented as executable objects with a
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

`executables` treats commands, queries, and events as related forms of application behavior rather than isolated
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

### 3.1. Executables and Executors

The first layer is the execution model itself. `IExecutable<TIn, TOut>` represents behavior as a reusable typed value,
while `IExecutor<TIn, TOut>` is the runtime invoker that performs `Execute(...)`.

That split is the foundation of the API design. You build and compose behavior at the executable level, then cross an
explicit execution boundary only when you need to run it. This keeps composition, adaptation, and decoration
(`Then/Fork/Map/Pipe/Apply/WithPolicy`) predictable and strongly typed.

In practical terms, this layer gives you:

- stable contracts for reusable business operations,
- clear separation between pipeline definition and invocation,
- symmetric sync/async modeling without changing the mental model.

### 3.2. Handleables: Query, Command, and Event

The second layer exposes domain-facing interaction contracts on top of the execution model. Instead of exposing raw
executables everywhere, this layer gives explicit API shapes:

- commands (`ICommand<T>` / `IAsyncCommand<T>`) for action semantics,
- queries (`IQuery<TIn, TOut>` / `IAsyncQuery<TIn, TOut>`) for request/response semantics,
- events (`IEvent<T>`) for publish/subscribe semantics.

Under the hood, base implementations (`Command`, `Query`, `Event`) are handleables. The core attachment contract is
`IHandleable<TIn, TOut, THandler>` (and async `IAsyncHandleable<...>`): behavior is provided externally through
`Handle(...)`, and attachment lifetime is controlled through returned `IDisposable`.

This is why one interaction contract can stay stable while its runtime behavior changes by swapping handlers. The
result is a clean division of responsibilities: contracts stay at the application boundary, execution logic remains
replaceable.

### 3.3. Operators and Policies

The third layer is operator composition over executors.

Operators wrap execution and can:

- preserve contracts (`BehaviorOperator` / `AsyncBehaviorOperator`),
- or adapt contracts (`ExecutionOperator` / `AsyncExecutionOperator`).

This is the mechanism behind context propagation, result wrapping, exception suppression, mapping, caching, metrics,
thread scheduling, and other cross-cutting behavior.

Policies are a focused operator family for execution rules: validation, guards, fallback, retry, timeout, and
reentrancy control. So policies are not a separate architecture branch; they are a semantic specialization inside the
operator layer.

## 4. Creating API Objects

### 4.1. Creating Executables with `Executable.Create(...)`

`Executable.Create(...)` is the main entry point for wrapping delegates into the library model. It supports functions
and actions with zero to four arguments.

For multi-argument delegates, the executable input becomes a tuple.

```csharp
IExecutable<int, int> square =
  Executable.Create((int x) => x * x);

IExecutable<(int, int), int> sum =
  Executable.Create((int a, int b) => a + b);

IExecutable<string, Unit> print =
  Executable.Create((string text) => Console.WriteLine(text));
```

### 4.2. Creating Async Executables with `AsyncExecutable.Create(...)`

`AsyncExecutable.Create(...)` is the async counterpart. It wraps async delegates into `IAsyncExecutable<...>` and
follows the same shape as the synchronous API.

```csharp
IAsyncExecutable<int, int> doubleAsync =
  AsyncExecutable.Create(async (int x, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return x * 2;
  });
```

### 4.3. Identity Executables

`Executable.Identity<T>()` and `AsyncExecutable.Identity<T>()` create pass-through executables that return the input
unchanged. They are useful in mapping, adaptation, and composition scenarios where one side of a transformation should
stay untouched.

```csharp
IExecutable<int, int> identity = Executable.Identity<int>();
int result = identity.GetExecutor().Execute(5); // 5
```

### 4.4. Converting Executables to Commands

If an executable returns `bool`, it can be exposed as a command through `AsCommand()`. This is the simplest way to reuse
an existing executable in a command-oriented API.

```csharp
ICommand<string> saveName =
  Executable.Create((string value) => !string.IsNullOrWhiteSpace(value))
    .AsCommand();
```

### 4.5. Converting Executables to Queries

Any executable can be exposed as a query through `AsQuery()`. This is useful when the operation is conceptually a read
or lookup and you want query-style semantics on top of reusable executable logic.

```csharp
IQuery<int, string> getUserName =
  Executable.Create((int id) => $"User-{id}")
    .AsQuery();
```

### 4.6. Converting Executables to Handlers

`AsHandler()` converts an executable into a `Handler<TIn, TOut>`. This is especially useful when working with base
`Command`, `Query`, or `Event` objects, because those types delegate their actual behavior to handlers.

```csharp
Handler<int, string> formatHandler =
  Executable.Create((int id) => $"User-{id}")
    .AsHandler();

var query = new Query<int, string>();
query.Handle(formatHandler);
```

### 4.7. Creating and Using Events

Events are created directly through `new Event<T>()`. Unlike commands and queries, they are not normally produced by
converting an executable. Instead, you create the event object, attach subscribers, and optionally attach a handler that
controls publication.

The library provides ready-to-use event publishers through `EventPublisher.Sequential(...)` and
`EventPublisher.Parallel(...)`. These handlers define how subscribers are invoked, while the event still owns the
subscriber list.

`EventPublisher.Sequential(...)` supports both direct and reverse order through `PublishOrder`. In direct order,
subscribers are called in the same order in which they are enumerated. In reverse order, the last subscribed handler
receives the message first.

Because `ISubscriber<T>` is also an `IExecutable<T, Unit>`, an executable can be converted into a subscriber through
`AsSubscriber()`.

```csharp
var changed = new Event<string>();

using IDisposable subscription = changed.Subscribe(value => Console.WriteLine(value));
using IDisposable executableSubscription = changed.Subscribe(
  Executable.Create((string value) => Console.WriteLine($"Executable: {value}"))
    .AsSubscriber());

changed.Handle(EventPublisher.Sequential<string>(PublishOrder.Reverse));

changed.Publish("Updated");
```

## 5. Composition API

### 5.1. Chaining with `Then(...)`

`Then(...)` is the basic way to compose steps into a pipeline. The output of one executable becomes the input of the
next one.

```csharp
IExecutable<string, int> parseLength =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.Length);
```

### 5.2. Branching with `Fork(...)`

`Fork(...)` sends one result into two parallel branches and returns both outputs as a tuple. This is useful when the
same intermediate value must be processed in multiple ways.

```csharp
IExecutable<string, (int, string)> info =
  Executable.Create((string text) => text.Trim())
    .Fork(
      text => text.Length,
      text => text.ToUpperInvariant());
```

### 5.3. Transforming Tuple Outputs with `First(...)`, `Second(...)`, and `Swap()`

After a fork, you can transform either side of the tuple independently or swap tuple items.

```csharp
IExecutable<string, (string, string)> transformed =
  Executable.Create((string text) => text.Trim())
    .Fork(text => text.Length, text => text.ToUpperInvariant())
    .First(length => $"Length: {length}")
    .Second(text => $"Value: {text}")
    .Swap();
```

### 5.4. Combining Branches with `Merge(...)`

`Merge(...)` turns a tuple result back into a single output. It is typically used after `Fork(...)`.

```csharp
IExecutable<string, string> summary =
  Executable.Create((string text) => text.Trim())
    .Fork(text => text.Length, text => text.ToUpperInvariant())
    .Merge((length, upper) => $"{upper} ({length})");
```

### 5.5. Adapting Contracts with `Map(...)`, `InMap(...)`, and `OutMap(...)`

These methods adapt an executable to a different external contract without changing its internal logic.

- `Map(...)` adapts both input and output,
- `InMap(...)` adapts only input,
- `OutMap(...)` adapts only output.

```csharp
IExecutable<int, int> square = Executable.Create((int x) => x * x);

IExecutable<string, string> squareText =
  square.Map(
    text => int.Parse(text),
    value => $"Result: {value}");
```

### 5.6. Observing Results with `Tap(...)`

`Tap(...)` runs a side effect on the result while preserving the original output. This is useful for logging, metrics,
or debugging.

```csharp
IExecutable<int, int> pipeline =
  Executable.Create((int x) => x * 2)
    .Tap(value => Console.WriteLine($"Produced: {value}"));
```

### 5.7. Building Reusable Pipelines with `Pipe(...)`

`Pipe(...)` lets you pass an executable through a reusable transformation function. This is useful when a composition
pattern should be applied in many places.

```csharp
IExecutable<int, string> pipeline =
  Executable.Create((int x) => x + 1)
    .Pipe(executable => executable.Then(value => $"Value: {value}"));
```

### 5.8. Connecting Queries with `Connect(...)`

`Connect(...)` composes two queries into one query by passing the output of the first query into the input of the
second query.

It supports sync-to-sync, async-to-async, and mixed sync/async combinations.

```csharp
IQuery<string, int> parse =
  Executable.Create((string text) => int.Parse(text))
    .AsQuery();

IQuery<int, string> format =
  Executable.Create((int value) => $"Value: {value}")
    .AsQuery();

IQuery<string, string> chained = parse.Connect(format);
string output = chained.Send("42"); // "Value: 42"
```

```csharp
IAsyncQuery<string, string> mixed =
  parse.Connect(
    AsyncExecutable.Create(async (int value, CancellationToken token) =>
    {
      await Task.Delay(1, token);
      return $"Async: {value}";
    }).AsQuery());
```

### 5.9. Composing Commands with `Compose(...)`

Commands are composed at the interface level through `Compose(...)`. This keeps command composition focused on the
command contract rather than on a specific concrete implementation.

`Compose(...)` supports:

- `ICommand<T> + ICommand<T> -> ICommand<T>`
- `ICommand<T> + IAsyncCommand<T> -> IAsyncCommand<T>`
- `IAsyncCommand<T> + ICommand<T> -> IAsyncCommand<T>`
- `IAsyncCommand<T> + IAsyncCommand<T> -> IAsyncCommand<T>`

Composite commands use short-circuit semantics: the second command is not executed when the first command returns
`false`.

```csharp
ICommand<string> first =
  Executable.Create((string value) =>
  {
    Console.WriteLine($"First: {value}");
    return true;
  }).AsCommand();

ICommand<string> second =
  Executable.Create((string value) =>
  {
    Console.WriteLine($"Second: {value}");
    return true;
  }).AsCommand();

ICommand<string> combined = first.Compose(second);
combined.Execute("Run");
```

```csharp
IAsyncCommand<string> asyncSecond =
  AsyncExecutable.Create(async (string value, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return true;
  }).AsCommand();

IAsyncCommand<string> mixed = first.Compose(asyncSecond);
await mixed.Execute("Run");
```

### 5.10. Branching Execution with `Branch` and `AsyncBranch`

For condition-based routing, the library provides `Branch...` and `AsyncBranch...`. They build an executable from
`If(...)`, optional `ElseIf(...)`, and terminal `Else(...)`.

Semantics:

- conditions are evaluated in order,
- only the first matching branch is executed,
- if none match, `Else(...)` is executed.

```csharp
int state = 1;

IQuery<Unit, string> stateText = Branch<string>
  .If(() => state == 0, () => "Init")
  .ElseIf(() => state == 1, () => "Running")
  .Else(() => "Unknown")
  .AsQuery();
```

```csharp
int state = 1;

IAsyncQuery<Unit, string> asyncStateText = AsyncBranch<string>
  .If(() => state == 0, async token =>
  {
    await Task.Delay(10, token);
    return "Init";
  })
  .Else(async token =>
  {
    await Task.Delay(10, token);
    return "Unknown";
  })
  .AsQuery();
```

## 6. Handleables and Handlers

### 6.1. Missing-Handler Behavior

The base implementations do not all react the same way when no handler is attached:

- `Command<T>` returns `false`,
- `Query<TIn, TOut>` throws `MissingHandlerException`,
- `Event<T>` does nothing if there are no subscribers, but throws `MissingHandlerException` if subscribers exist and no
  handler is attached.

This distinction is important when choosing which abstraction to expose.

### 6.2. Subscription Lifetime

Event subscriptions are explicit and disposable. `Subscribe(...)` returns an `IDisposable`, and disposing it removes the
subscriber from the event.

```csharp
var changed = new Event<string>();
using IDisposable subscription = changed.Subscribe(value => Console.WriteLine(value));
```

### 6.3. One-Time Subscriptions

`SubscribeOnce(...)` is useful for fire-once reactions such as initialization notifications, confirmation events, or
single-use UI hooks.

```csharp
var changed = new Event<string>();
changed.SubscribeOnce(value => Console.WriteLine($"First: {value}"));
```

### 6.4. Parameterless Operations

Commands, queries, and events all support parameterless usage through `Unit`-based overloads.

```csharp
ICommand<Unit> refresh =
  Executable.Create(() => Console.WriteLine("Refresh"))
    .Then(_ => true)
    .AsCommand();

IQuery<Unit, string> getVersion =
  Executable.Create(() => "1.0.0")
    .AsQuery();

IEvent<Unit> tick = new Event<Unit>();

refresh.Execute();
string version = getVersion.Send();
tick.Publish();
```

### 6.5. Handleables and Merging

`IHandleable<...>` represents an object that can accept a handler through `Handle(...)`. This is the common attachment
model behind base types such as `Command<T>`, `Query<TIn, TOut>`, and `Event<T>`.

This abstraction is useful when the important part is not the specific command or query contract, but the fact that an
object can be connected to execution logic from the outside.

`IHandleable<...>` objects can also be merged with `Merge(...)`. This lets multiple handleables share one attached
handler. In other words, `Merge(...)` combines handler attachment points rather than command behavior.

```csharp
IHandleable<string, Unit> first = new Command<string>();
IHandleable<string, Unit> second = new Command<string>();

var merged = first.Merge(second);

merged.Handle(value => Console.WriteLine(value));
```

`Handle(...)` itself returns an `IDisposable` attachment handle, so the handler can later be detached from the merged
handleable. The extension overloads also allow attaching plain delegates directly, without manually converting them to
handlers first.

### 6.6. Handler Lifecycle and Disposal

`Handler<TIn, TOut>` and `AsyncHandler<TIn, TOut>` share a common disposal model through `DisposableHandler`. A handler
exposes a `Disposed` flag, tracks all active attachment handles, and automatically detaches from connected handleables
when it is disposed.

This matters because disposal is not only about releasing resources. It also closes the handler as an execution
endpoint. After disposal, invoking it throws `HandlerDisposedException`.

The API also provides disposal helpers:

- `OnDispose(...)` adds custom disposal logic,
- `DisposeOnUnhandledException()` wraps a handler so that this wrapped attachment is disposed when an exception escapes
  from its execution logic.

If the same handler instance is attached to multiple handleables, disposing that handler detaches all of its active
attachments. Because of that, exception-based auto-disposal is best used when any unhandled exception should be treated
as fatal for the handler instance itself, not for ordinary input or validation failures.

Applied at attachment time, `DisposeOnUnhandledException()` affects only that wrapped handler instance. If you want a
shared handler itself to become invalid after an unhandled exception, apply `DisposeOnUnhandledException()` when
creating the shared handler, not only when attaching it to one handleable.

```csharp
var first = new Query<int, string>();
var second = new Query<int, string>();

Handler<int, string> sharedHandler =
  Executable.Create((int id) =>
    {
      string[] values = ["zero", "one"];
      return values[id].ToUpperInvariant();
    }).AsHandler();

first.Handle(sharedHandler);
second.Handle(sharedHandler.DisposeOnUnhandledException());

second.Send(10);
// The wrapped attachment used by `second` is disposed.
// The original `sharedHandler` remains attached to `first`.
```

```csharp
Handler<int, string> fatalSharedHandler =
  Executable.Create((int id) =>
  {
    string[] values = ["zero", "one"];
    return values[id].ToUpperInvariant();
  })
  .AsHandler()
  .DisposeOnUnhandledException();

first.Handle(fatalSharedHandler);
second.Handle(fatalSharedHandler);

second.Send(10);
// The shared handler instance is disposed,
// so it is detached from both `first` and `second`.
```

## 7. Execution Operators

### 7.1. Operator Model

Operators are wrappers around executors. They define how execution is transformed before and after the wrapped
executable runs.

The API is built around four base abstractions:

- `ExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`,
- `BehaviorOperator<TIn, TOut>`,
- `AsyncExecutionOperator<TInOp, TInExec, TOutExec, TOutOp>`,
- `AsyncBehaviorOperator<TIn, TOut>`.

`BehaviorOperator` variants keep the same contract (`TIn -> TOut`). `ExecutionOperator` variants can adapt input and
output contracts around the wrapped executor.

### 7.2. Applying Operators with `Apply(...)`

`Apply(...)` is the low-level attachment point for operators.

- `executable.Apply(operator)` and `operator.Apply(executable)` are both supported for sync and async APIs,
- the returned executable keeps strong typing across all transformations.

```csharp
IExecutable<string, string> pipeline =
  Executable.Create((int value) => value * 2)
    .Apply(ExecutionOperator.Map(
      Executable.Create((string text) => int.Parse(text)),
      Executable.Create((int value) => $"Value: {value}")));      
```

### 7.3. Operator Factories: `Create(...)`, `Map(...)`

For explicit operator construction, the static `ExecutionOperator` / `AsyncExecutionOperator` APIs provide:

- `Create(...)` for custom operator logic from a delegate,
- `Map(...)` for contract adaptation around another executable.

This factory style is useful when operator behavior should be reused as a standalone unit instead of being inlined in
an extension method chain.

```csharp
ExecutionOperator<string, int, int, string> formatSquared =
  ExecutionOperator.Create((string text, IExecutor<int, int> next) =>
  {
    int value = int.Parse(text);
    int squared = next.Execute(value);
    return $"Squared: {squared}";
  });

IExecutable<int, int> squared = Executable.Create((int value) => value * value);

IExecutable<string, string> pipeline = squared.Apply(formatSquared);
pipeline = formatSquared.Apply(squared);

pipeline = Executable.Create((int value) => value * 2)
  .Map((string text) => int.Parse(text), value => $"Value: {value}");
```

### 7.4. Context and Result Operators

High-level operator-based extensions include:

- `WithContext(...)` for context-scoped execution (`ExecutableContext`),
- `WithResult()` for converting exceptions to `Result<T>`.

These are execution decorators, not business operations. They keep core logic focused while adding predictable runtime
behavior around it.

```csharp
IExecutable<int, Result<string>> query =
  Executable.Create((int id) =>
  {
    string tenant = ExecutableContext.Current.Get<string>("tenant");
    return $"{tenant}:{id}";
  })
  .WithContext(context => context.Set("tenant", "eu-1"))
  .WithResult();
```

### 7.5. Exception and Scheduling Operators

Additional operator-based extensions:

- `SuppressException().OfType<TEx>()` to convert selected exceptions to `Optional<T>`,
- `OnThreadPool()` for scheduling `IExecutable<T, Unit>` on the thread pool.

Both modify execution semantics without changing business contracts.

```csharp
IExecutable<string, Optional<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .SuppressException()
    .OfType<FormatException>();

IExecutable<string, Unit> notify =
  Executable.Create((string message) => Console.WriteLine(message))
    .OnThreadPool();
```

### 7.6. Cache and Metrics Operators

The low-level operator factory includes reusable runtime wrappers:

- `ExecutionOperator.Cache(...)` / `AsyncExecutionOperator.Cache(...)`,
- `ExecutionOperator.Metrics(...)` / `AsyncExecutionOperator.Metrics(...)`.

For convenience, the same behavior is exposed as executable extension methods: `Cache(...)` and `Metrics(...)` for sync
and async executable chains.

```csharp
ICacheStorage<int, string> cacheStorage = /* your storage */;
IMetrics<int, string> metrics = /* your metrics sink */;

IExecutable<int, string> loadUser =
  Executable.Create((int id) => $"User-{id}")
    .Cache(cacheStorage)
    .Metrics(metrics, tag: "users.load");
```

### 7.7. Pipelines and Middlewares

`Pipeline` and `AsyncPipeline` are builder APIs for middleware-style composition. Each middleware is an execution
operator that receives input plus a `next` delegate, can transform data before and after `next`, and can stop the chain
by not calling it.

This model is useful when you want explicit around-execution behavior (logging, timing, normalization, enrichment)
without manually nesting decorators.

For `Use(...)` overloads, explicitly typing lambda parameters is recommended to avoid overload ambiguity.

`Use(...)` chains are unbounded and fully type-safe: each step can change types and choose the most convenient `next`
delegate shape (function/action, parameterized/parameterless). For each adjacent pair, contracts must match: the
`next` delegate used in step `N` is defined by step `N + 1`.

The main limitation is that one chain cannot mix sync and async middleware: use either `Pipeline...` or
`AsyncPipeline...` for a given chain.

```csharp
IQuery<string, string> query = Pipeline<string, string>
  .Use((string text, Func<TimeSpan, int> next) =>
  {
    TimeSpan parsed = TimeSpan.Parse(text);
    int seconds = next(parsed);
    return $"Seconds: {seconds}";
  })
  .Use((TimeSpan span, Func<double, int> next) =>
  {
    double minutes = span.TotalMinutes;
    return next(minutes);
  })
  .End(minutes => (int)(minutes * 60))
  .AsQuery();
```

```csharp
IAsyncQuery<string, string> asyncQuery = AsyncPipeline<string, string>
  .Use(async (string text, AsyncFunc<int, int> next, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    int doubled = await next(text.Trim().Length, token);
    return $"Length: {doubled}";
  })
  .End(async (length, token) =>
  {
    await Task.Delay(10, token);
    return length * 2;
  })
  .AsQuery();
```

### 7.8. Writing Custom Operators

When built-in operators are not enough, create a custom operator by:

1. inheriting from one of the base operator types, or
2. using `ExecutionOperator.Create(...)` / `AsyncExecutionOperator.Create(...)` for delegate-based operators.

This keeps custom execution behavior explicit and composable, while preserving the same typed `Apply(...)` model used
by the rest of the library.

```csharp
IExecutable<string, string> trimmed =
  Executable.Create((string value) => value)
    .Apply(ExecutionOperator.Create((string input, IExecutor<string, string> next) =>
      next.Execute(input.Trim())));
```

## 8. Policies and Execution Control

### 8.1. Applying Policies with `WithPolicy(...)`

`WithPolicy(...)` wraps an executable with a policy pipeline built through `PolicyBuilder<TIn, TOut>` or
`AsyncPolicyBuilder<TIn, TOut>`. This is the main entry point for validation, guards, fallback behavior, retry rules,
timeouts, and execution restrictions.

Policies are added declaratively, but they run as wrappers around the executable. The last added policy executes first.

```csharp
IExecutable<string, int> parse =
  Executable.Create((string text) => int.Parse(text))
    .WithPolicy(policy =>
    {
      policy.ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required");
      policy.Fallback<FormatException>((text, ex) => 0);
    });
```

### 8.2. Input and Output Validation

Validation policies are applied through `Validate(...)`, `ValidateInput(...)`, and `ValidateOutput(...)`. They are a
better place for contract checks than embedding validation directly into handler logic.

```csharp
IExecutable<string, int> parseLength =
  Executable.Create((string text) => text.Trim().Length)
    .WithPolicy(policy =>
    {
      policy.ValidateInput(text => text.Length <= 100, "Input is too long");
      policy.ValidateOutput(length => length >= 0, "Length must be non-negative");
    });
```

### 8.3. Guard Conditions

Guards block execution when some external condition is not satisfied. Unlike validators, they usually express access or
state constraints rather than input correctness. When a guard denies execution, the policy throws
`AccessDeniedException`.

```csharp
bool isEnabled = true;

IExecutable<Unit, string> run =
  Executable.Create(() => "Started")
    .WithPolicy(policy => policy.Guard(() => isEnabled, "Operation is disabled"));
```

### 8.4. Retry, Timeout, and Fallback Patterns

The async policy builder supports retry and timeout control, while both sync and async builders support fallback.

Use fallback when a known exception should be converted into a result. Use retry when an operation may succeed on a
later attempt. Use timeout when total async execution time must be bounded.

```csharp
IAsyncExecutable<int, string> load =
  AsyncExecutable.Create(async (int id, CancellationToken token) =>
  {
    await Task.Delay(50, token);
    return $"User-{id}";
  })
  .WithPolicy(policy =>
  {
    policy.Timeout(TimeSpan.FromSeconds(2));
    policy.Retry<TimeoutException>(RetryRule.ExponentialBackoff<TimeoutException>(
      TimeSpan.FromMilliseconds(100),
      maxAttempts: 3));
    policy.Fallback<TimeoutException>((id, ex) => "Unavailable");
  });
```

### 8.5. Preventing Reentrancy

`PreventReentrance()` blocks nested re-entry into the same executable. Its main purpose is to prevent a call from
invoking the same executable again while the original call is still in progress.

This is different from general concurrency control. The important case here is recursive or indirect self-invocation,
where executable logic loops back into itself through another call path.

```csharp
IExecutable<int, int> recursive = null;

recursive =
  Executable.Create((int value) =>
    value == 0
      ? 0
      : recursive.GetExecutor().Execute(value - 1))
    .WithPolicy(policy => policy.PreventReentrance());
```

### 8.6. Validator API: Primitives and Composition

The `Validator` API can be used directly, not only through policy shorthand methods.

Primitive validators are the direct comparison predicates:

- `Equal`, `MoreThan`, `LessThan`, `MoreThanOrEqual`, `LessThanOrEqual`.

Composition validators include `And(...)`, `Or(...)`, and `Validator.Not(...)`. Error messages can be customized with
`OverrideMessage(...)`.

Derived/domain-sugar validators are built from primitives and composition:

- `NotEqual` (negated `Equal`),
- `ZeroEqual`, `ZeroNotEqual`, `MoreThanZero`, `LessThanZeroOrEqual`,
- `InRange` / `OutRange`,
- `NotEmptyString`, `NotEmptyCollection`.

Specialized validators:

- `NotNull`,
- `StringLength(...)`, `CollectionCount(...)`, `All(...)`, `Any(...)`, `Is<T>()`, `Match(...)`,
- `Create(predicate, errorMessage)` for custom rules.

```csharp
Validator<int> ageValidator =
  Validator.InRange(18, 100, rightInclusive: true)
    .And(Validator.NotEqual(42))
    .Or(Validator.Equal(120))
    .OverrideMessage("Age is not allowed");

IExecutable<int, string> flow =
  Executable.Create((int age) => $"Accepted: {age}")
    .WithPolicy(policy => policy.ValidateInput(ageValidator));
```

### 8.7. Guard API: Factory and Composition

Besides predicate overloads on policy builders, guards have a dedicated API.

- `Guard.Create(condition, message)` creates a guard from a predicate,
- `Guard.Identity()` always allows access,
- `Guard.Manual(message)` creates a `ToggleGuard` with runtime-switchable access (`Deny()` blocks, `Allow()` allows),
- `guardA.Compose(guardB)` combines guards with logical AND semantics.

```csharp
ToggleGuard maintenance = Guard.Manual("Service is unavailable");
Guard licensed = Guard.Create(() => hasLicense, "License required");
Guard guard = maintenance.Compose(licensed);

IExecutable<Unit, string> run =
  Executable.Create(() => "OK")
    .WithPolicy(policy => policy.Guard(guard));

maintenance.Deny(); // next call will fail with AccessDeniedException
```

### 8.8. Retry Rules API

Retry behavior is configured in async policies through `IRetryRule<TException>`.

- `RetryRule.Create(...)` builds a custom rule from delegate,
- `RetryRule.ExponentialBackoff<TEx>(...)` provides delay-based retries.

```csharp
IRetryRule<InvalidOperationException> rule =
  RetryRule.ExponentialBackoff<InvalidOperationException>(
    TimeSpan.FromMilliseconds(50),
    maxAttempts: 5);
// Up to 5 retries with exponential backoff delays.

IAsyncExecutable<int, int> flow =
  AsyncExecutable.Create(async (int value, CancellationToken token) =>
  {
    await Task.Delay(10, token);
    return value * 2;
  })
  .WithPolicy(policy => policy.Retry(rule));
```

## 9. Context, Safety, and Error Handling

### 9.1. Running with `WithContext(...)`

`WithContext(...)` runs an executable inside a new `ExecutableContext`. You can initialize that context with
`ContextWriter` (`Name`, `Set(...)`) before the main logic starts.

Inside execution, the current context is available through `ExecutableContext.Current`.

```csharp
IQuery<int, string> query =
  Executable.Create((int id) =>
  {
    string tenant = ExecutableContext.Current.Get<string>("tenant");
    return $"{tenant}:{id}";
  })
  .WithContext(context =>
  {
    context.Name = "GetUser";
    context.Set("tenant", "eu-1");
  })
  .AsQuery();
```

### 9.2. Correlation and Nested Execution Contexts

Nested `WithContext(...)` calls create nested contexts. The nested context gets a new `ContextId`, increased `Depth`,
and keeps the same `CorrelationId` from the parent context.

Values are resolved from the current context first, then up the parent chain.

```csharp
IQuery<Unit, Guid> inner =
  Executable.Create(() => ExecutableContext.Current.CorrelationId)
    .WithContext(context => context.Name = "Inner")
    .AsQuery();

IQuery<Unit, Guid> outer =
  Executable.Create(() =>
  {
    Guid outerCorrelation = ExecutableContext.Current.CorrelationId;
    Guid innerCorrelation = inner.Send();
    return outerCorrelation == innerCorrelation ? outerCorrelation : Guid.Empty;
  })
  .WithContext(context => context.Name = "Outer")
  .AsQuery();
```

### 9.3. Suppressing Exceptions with `Optional<T>`

`SuppressException().OfType<TEx>()` suppresses only the selected exception type and converts output to `Optional<T>`.
Other exception types still propagate.

```csharp
IQuery<string, Optional<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .SuppressException()
    .OfType<FormatException>()
    .AsQuery();
```

You can chain multiple suppressions for different exception types.

`Optional<T>` is the lightweight result shape for "value exists / value is absent" flows.

- check `HasValue` before reading `Value`,
- use `ValueOrDefault` when a fallback default is acceptable.

```csharp
Optional<int> response = parse.Send("oops");

if (response.HasValue)
  Console.WriteLine(response.Value);
else 
  Console.WriteLine(response.ValueOrDefault); // default(int) == 0
```

### 9.4. Wrapping Execution with `WithResult()`

`WithResult()` converts exceptions into `Result<T>` so callers can handle failures without `try/catch` at every call
site.

Unlike `Optional<T>`, `Result<T>` does not hide the error. On failure it stores the original exception in
`Result<T>.Exception`, so error details are preserved and can be inspected or rethrown through `ThrowIfFailure()`.

```csharp
IQuery<string, Result<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .WithResult()
    .AsQuery();

Result<int> ok = parse.Send("42");   // IsSuccess == true
Result<int> fail = parse.Send("bad"); // IsFailure == true

if (fail.IsFailure)
{
  Console.WriteLine(fail.Exception.GetType().Name); // FormatException
  // fail.ThrowIfFailure(); // rethrows original exception
}
```

This works for both sync and async executables.

### 9.5. Disposal and Lifetime Considerations

`WithContext(...)` always restores the previous `ExecutableContext.Current` in `finally`, even when initialization or
execution throws. The temporary context is disposed at the end of the call.

That means:

- `ExecutableContext.Current` should be treated as execution-scoped ambient state,
- do not cache context instances outside the running call,
- after the call completes, outer code sees the previous context (often `null`).

For handler lifetimes and attachment disposal, use the lifecycle rules from section 6.6.

## 10. Synchronous and Asynchronous Usage

### 10.1. `IExecutable` vs `IAsyncExecutable`

`IExecutable<TIn, TOut>` is a synchronous contract. It is simpler when work is CPU-bound and completes immediately.

`IAsyncExecutable<TIn, TOut>` is an asynchronous contract. It is designed for I/O and long-running operations and
supports cancellation tokens.

In practice:

- sync path returns `TOut`,
- async path returns `ValueTask<TOut>`.

### 10.2. `ToAsyncExecutable()`

`ToAsyncExecutable()` wraps a synchronous executable into an async proxy, so it can participate in async chains without
rewriting existing logic.

It adapts the contract, not the business behavior.

```csharp
IExecutable<int, int> square = Executable.Create((int x) => x * x);
IAsyncExecutable<int, int> squareAsync = square.ToAsyncExecutable();

int value = await squareAsync.GetExecutor().Execute(5); // 25
```

### 10.3. `ToAsyncCommand()` and `ToAsyncQuery()`

`ToAsyncCommand()` and `ToAsyncQuery()` do the same contract adaptation for command and query interfaces.

```csharp
ICommand<string> save = Executable.Create((string value) => value.Length > 0).AsCommand();
IAsyncCommand<string> saveAsync = save.ToAsyncCommand();

IQuery<int, string> getName = Executable.Create((int id) => $"User-{id}").AsQuery();
IAsyncQuery<int, string> getNameAsync = getName.ToAsyncQuery();
```

### 10.4. Mixing Sync and Async Chains

The API supports mixed composition directly:

- `IExecutable.Then(IAsyncExecutable)` produces `IAsyncExecutable`,
- `IAsyncExecutable.Then(IExecutable)` stays `IAsyncExecutable`,
- `IQuery.Connect(IAsyncQuery)` and `IAsyncQuery.Connect(IQuery)` are supported,
- `ICommand.Compose(IAsyncCommand)` and `IAsyncCommand.Compose(ICommand)` are supported.

```csharp
IExecutable<string, int> parse = Executable.Create((string text) => int.Parse(text));

IAsyncExecutable<string, string> mixed =
  parse.Then(async (int x, CancellationToken token) =>
  {
    await Task.Delay(1, token);
    return $"Value: {x}";
  });
```

### 10.5. Choosing the Right API Style

Use sync by default when operations are fast and local. Use async when execution can block on external resources or
must support cancellation.

A practical approach is to keep core pure transforms synchronous, then switch to async at boundaries (HTTP, DB, file
I/O, messaging). If a flow may become async soon, converting with `ToAsync...()` lets you migrate incrementally without
rewriting composition code.

## 11. Common Usage Patterns

### 11.1. Building a Simple Processing Pipeline

Start from a small executable, then compose steps with `Then(...)`, and expose the final chain as a query.

```csharp
IQuery<string, string> normalizeAndFormat =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.ToUpperInvariant())
    .Then(text => $"Value: {text}")
    .AsQuery();
```

### 11.2. Wrapping Existing Delegates into the Library Model

Existing delegates can be wrapped first, then reused across command/query/handler APIs without rewriting logic.

```csharp
Func<int, bool> canProcess = value => value > 0;
IExecutable<int, bool> executable = Executable.Create(canProcess);

ICommand<int> command = executable.AsCommand();
Handler<int, bool> handler = executable.AsHandler();
```

### 11.3. Adding Cross-Cutting Rules Around Business Logic

Keep business logic focused and attach cross-cutting behavior with policies and operators.

```csharp
IExecutable<string, Result<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .WithPolicy(policy =>
    {
      policy.ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required");
      policy.Fallback<FormatException>((text, ex) => 0);
    })
    .WithResult();
```

### 11.4. Using Events for Notification Flows

Use `Event<T>` when one publisher should notify multiple subscribers through a selected publisher strategy.

```csharp
var changed = new Event<string>();

changed.Handle(EventPublisher.Sequential<string>());
changed.Subscribe(value => Console.WriteLine($"A: {value}"));
changed.Subscribe(value => Console.WriteLine($"B: {value}"));

changed.Publish("Updated");
```

### 11.5. Implementing Undo/Redo Behavior

Use `ReversibleCommand<TInput, TChange>` when a command must produce reversible changes and keep history.

```csharp
var values = new List<int>();
var command = new ReversibleCommand<int, int>();

command.Handle(ReversibleHandler.Create<int>(
  execution: value => values.Add(value),
  undo: value => values.Remove(value)));

command.Execute(10);
command.Undo();
command.Redo();
```

When change data is not equal to input, use `Change<T>` (or your own change model) to keep previous/new state
explicitly.

```csharp
int current = 0;
var assign = new ReversibleCommand<int, Change<int>>();

assign.Handle(ReversibleHandler.Create<int, Change<int>>(
  execution: value =>
  {
    var change = new Change<int>(current, value);
    current = value;
    return change;
  },
  undo: change => current = change.Old,
  redo: change => current = change.New));

assign.Execute(10); // current = 10
assign.Undo();      // current = 0
assign.Redo();      // current = 10
```

### 11.6. Integrating with UI Actions or Application Services

Map UI/application inputs to commands and queries, then keep orchestration in executable composition instead of in UI
callbacks.

```csharp
public readonly record struct User(int Id, string Name);

var users = new Dictionary<int, User>();

ICommand<User> saveUser =
  Executable.Create((User user) =>
  {
    if (string.IsNullOrWhiteSpace(user.Name))
      return false;

    users[user.Id] = user;
    return true;
  })
    .AsCommand();

IQuery<int, User> loadUser =
  Executable.Create((int id) => users.TryGetValue(id, out User user) ? user : throw new UnknownUserException())
    .AsQuery();

bool saved = saveUser.Execute(new User(1, "Denis"));
User user = loadUser.Send(1); // User { Id = 1, Name = "Denis" }
```

## 12. API Quick Reference

### 12.1. Static Entry Points

### 12.2. Most Important Extension Methods

- `Executable.Create(...)`, `AsyncExecutable.Create(...)`: static entry points for creating executables.
- `AsCommand()`, `AsQuery()`, `AsHandler()`, `AsSubscriber()`: conversion methods on executable/query/command types.
- `Then(...)`, `Fork(...)`, `Merge(...)`, `Map(...)`, `Pipe(...)`, `Tap(...)`: composition methods on
  `IExecutable<...>` / `IAsyncExecutable<...>`.
- `Connect(...)`: query composition on `IQuery<...>` / `IAsyncQuery<...>` (`sync/sync`, `sync/async`,
  `async/sync`, `async/async`).
- `Compose(...)`: command composition on `ICommand<T>` / `IAsyncCommand<T>` (`sync/sync`, `sync/async`,
  `async/sync`, `async/async`).
- `WithPolicy(...)`: policy composition on `IExecutable<...>` / `IAsyncExecutable<...>`.
- `WithContext(...)`, `WithResult()`, `SuppressException()`: execution operators on executable types.
- `Cache(...)`, `Metrics(...)`: operator shortcuts on `IExecutable<...>` / `IAsyncExecutable<...>`.
- `DisposeOnUnhandledException()`, `OnDispose(...)`: lifecycle helpers on `Handler<...>` / `AsyncHandler<...>`.
- `OnThreadPool()`: execution operator for `IExecutable<T, Unit>`.

### 12.3. Most Important Return Types

### 12.4. Recommended Starting Points for New Users

## 13. Practical Notes

### 13.1. Target Frameworks

### 13.2. Package Usage Expectations

### 13.3. Thread-Safety Notes

### 13.4. Performance and Allocation Considerations

### 13.5. Limitations and Tradeoffs

## 14. Suggested Expansion Order

### 14.1. Overview and Core Concepts First

### 14.2. Creation and Composition API Next

### 14.3. Policies, Context, and Error Handling After That

### 14.4. Code Examples Where They Matter Most
