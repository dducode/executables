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

Add cross-cutting behavior with operators or policies when needed.
