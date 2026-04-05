# Getting Started

Create an executable from a delegate:

```csharp
IExecutable<int, int> square =
  Executable.Create((int value) => value * value);
```

Compose it into a pipeline:

```csharp
IExecutable<string, string> pipeline =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.Length)
    .Then(length => $"Length: {length}");
```

Expose the same logic as a query:

```csharp
IQuery<string, string> query = pipeline.AsQuery();
string result = query.Send("  demo  ");
```

Add cross-cutting runtime behavior on the executor when needed:

```csharp
IExecutor<string, Result<string>> runtime =
  pipeline
    .GetExecutor()
    .WithPolicy(policy => policy.ValidateInput(text => text.Length <= 100, "Input is too long"))
    .WithResult();
```
