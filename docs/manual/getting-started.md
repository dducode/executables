# Getting Started

The quickest way to understand the library is to walk one piece of logic through its full lifecycle:

1. create an executable from a delegate,
2. compose it with other executable steps,
3. expose it as a query or command if needed,
4. move to an executor when runtime behavior becomes relevant.

Start with a single executable:

```csharp
IExecutable<string, string> trim =
  Executable.Create((string text) => text.Trim());
```

Then compose it into a larger flow:

```csharp
IExecutable<string, string> pipeline =
  trim
    .Then(text => text.Length)
    .Then(length => $"Length: {length}");
```

At this stage the logic is still pure composition. If the flow represents a query, expose it as one:

```csharp
IQuery<string, string> query = pipeline.AsQuery();
string result = query.Send("  demo  ");
```

When execution needs validation, policies, result wrapping, or other runtime concerns, switch to an executor:

```csharp
IExecutor<string, Result<string>> runtime =
  pipeline
    .GetExecutor()
    .WithPolicy(policy => policy.ValidateInput(text => text.Length <= 100, "Input is too long"))
    .WithResult();
```

Notice what stays stable here: `pipeline` itself does not change. You can execute the same composition through
different executors, each with its own runtime behavior.

That is the main split in the library:

- `IExecutable<TIn, TOut>` models composition,
- `IExecutor<TIn, TOut>` models execution.
