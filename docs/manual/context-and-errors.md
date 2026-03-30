# Context and Errors

## Execution Context

`WithContext(...)` runs an executable inside a new `ExecutableContext`.

Inside execution, the ambient context is available through `ExecutableContext.Current`.

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

Nested contexts keep correlation while creating a deeper scope.

## Optional and Result

`Optional<T>` and `Result<T>` represent two different error-handling strategies.

`Optional<T>` is used when selected exception types should be suppressed and converted into absence of value.

```csharp
IQuery<string, Optional<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .SuppressException()
    .OfType<FormatException>()
    .AsQuery();
```

`Result<T>` preserves failure information instead of suppressing it.

```csharp
IQuery<string, Result<int>> parseResult =
  Executable.Create((string text) => int.Parse(text))
    .WithResult()
    .AsQuery();
```

## Disposal Considerations

Execution context is always restored in `finally`, even when initialization or execution throws.

For handler lifetimes and subscription disposal rules, use the lifecycle model described in the handleables and handlers
section.
