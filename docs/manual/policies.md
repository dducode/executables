# Policies

Policies are a specialized runtime family focused on execution rules rather than on business logic.
They are applied to executors, not to executable composition objects.

## Applying Policies

`WithPolicy(...)` is the main entry point for policy composition on `IExecutor` / `IAsyncExecutor`.

The builder supports both block-style configuration and fluent chaining. Fluent style is usually more compact for
straight-line policy pipelines.

```csharp
IExecutor<string, int> parse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .Fallback<FormatException>((text, ex) => 0));
```

## Validation

Validation policies belong around execution boundaries, not inside handlers.

```csharp
IExecutor<string, int> parseLength =
  Executable.Create((string text) => text.Trim().Length)
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => text.Length <= 100, "Input is too long")
      .ValidateOutput(length => length >= 0, "Length must be non-negative"));
```

## Guards

Guards express external state or access rules. When a guard denies execution, it throws `AccessDeniedException`.

```csharp
bool isEnabled = true;

IExecutor<Unit, string> run =
  Executable.Create(() => "Started")
    .GetExecutor()
    .WithPolicy(policy => policy.Guard(() => isEnabled, "Operation is disabled"));
```

## Retry, Timeout, and Fallback

Async policies support timeout and retry; both sync and async policies support fallback.

```csharp
IAsyncExecutor<int, string> load =
  AsyncExecutable.Create(async (int id, CancellationToken token) =>
  {
    await Task.Delay(50, token);
    return $"User-{id}";
  })
  .GetExecutor()
  .WithPolicy(policy => policy
    .Timeout(TimeSpan.FromSeconds(2))
    .Retry<TimeoutException>(
      RetryRule.ExponentialBackoff<TimeoutException>(TimeSpan.FromMilliseconds(100), maxAttempts: 3))
    .Fallback<TimeoutException>((id, ex) => $"User-{id} is temporarily unavailable"));
```

## Reentrancy Control

`PreventReentrance()` blocks nested re-entry into the same executable while an earlier call is still running.

## Validator, Guard, and Retry APIs

The library also exposes lower-level building blocks:

- validators with primitive, composed, and domain-sugar rules,
- guards with factory and composition APIs,
- retry rules through `IRetryRule<TException>` and `RetryRule`.
