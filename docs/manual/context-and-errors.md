# Context and Errors

## Execution Context

`WithContext(...)` runs an executor inside a new `ExecutableContext`.

Inside execution, the ambient context is available through `ExecutableContext.Current`.

```csharp
IExecutor<int, string> query =
  Executable.Create((int id) =>
  {
    string tenant = ExecutableContext.Current.Get<string>("tenant");
    return $"{tenant}:{id}";
  })
  .GetExecutor()
  .WithContext(context =>
  {
    context.Name = "GetUser";
    context.Set("tenant", "eu-1");
  });
```

Nested contexts keep correlation while creating a deeper scope.

## Optional and Result

`Optional<T>` and `Result<T>` represent two different error-handling strategies.

`Optional<T>` is used when selected exception types should be suppressed and converted into absence of value.

```csharp
IExecutor<string, Optional<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .SuppressException()
    .OfType<FormatException>();
```

`Result<T>` preserves failure information instead of suppressing it.

```csharp
IExecutor<string, Result<int>> parseResult =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .WithResult();
```

## Disposal Considerations

Execution context is always restored in `finally`, even when initialization or execution throws.

For handler lifetimes and subscription disposal rules, use the lifecycle model described in the handleables and handlers
section.
