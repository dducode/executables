# Introduction

`Executables` is a composable .NET library for modeling application behavior as reusable executable units.

The core idea is simple: define behavior once as composition, and decide later how that behavior should run.

That makes it possible to keep the same composition while executing it through different runtimes, with different
policies, operators, context setup, or error-handling behavior.

The library centers around a few related conceptual areas:

- `Executable` for pure composition and contract shape
- `Query` and `Command` as specialized executable contracts built around executable composition
- `Executor` for runtime behavior, policies, operators, and execution control
- `Event` and `Subscriber` as the publication/subscription model that overlaps executable and interaction concerns
- `Handleable` and `Handler` as optional attachment-oriented abstractions in the same broader interaction space

As a rule of thumb:

- start with `Executable`, `Then(...)`, and `Compose(...)` when defining behavior,
- move to `Executor` when execution needs policies, operators, context, or error handling,
- use `Query`, `Command`, `Event`, `Subscriber`, `Handleable`, and `Handler` only when the application boundary calls for them.

The rest of the manual expands this model from different angles:

- `Getting Started` shows the smallest useful workflow,
- `Overview` explains what the library is good at,
- `Conceptual Model` explains how the abstractions fit together.
