# Introduction

`Executables` is a composable .NET library for modeling application behavior as reusable executable units.

The library centers around a few related conceptual areas:

- `Executable` for pure composition and contract shape
- `Executor` for runtime behavior, policies, and execution control
- `Query` and `Command` as specialized executable contracts
- `Event` and `Subscriber` as the publication/subscription model
- `Handleable` and `Handler` as optional attachment-oriented abstractions
- operators and policies as runtime decorators on executors

The manual section will hold the conceptual and usage-oriented documentation. Generated API reference is published
separately from XML documentation in the codebase.
