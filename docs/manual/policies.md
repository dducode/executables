# Policies

Policies are a specialized operator family focused on execution rules rather than on business logic.

## Applying Policies

`WithPolicy(...)` is the main entry point for policy composition.

The builder supports both block-style configuration and fluent chaining. Fluent style is usually more compact for
straight-line policy pipelines.

```csharp
IExecutable<string, int> parse =
  Executable.Create((string text) => int.Parse(text))
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .Fallback<FormatException>((text, ex) => 0));
```

## Validation

Validation policies belong around executable boundaries, not inside handlers.

```csharp
IExecutable<string, int> parseLength =
  Executable.Create((string text) => text.Trim().Length)
    .WithPolicy(policy => policy
      .ValidateInput(text => text.Length <= 100, "Input is too long")
      .ValidateOutput(length => length >= 0, "Length must be non-negative"));
```

## Guards

Guards express external state or access rules. When a guard denies execution, it throws `AccessDeniedException`.

```csharp
bool isEnabled = true;

IExecutable<Unit, string> run =
  Executable.Create(() => "Started")
    .WithPolicy(policy => policy.Guard(() => isEnabled, "Operation is disabled"));
```

## Retry, Timeout, and Fallback

Async policies support timeout and retry; both sync and async policies support fallback.

```csharp
IAsyncExecutable<int, string> load =
  AsyncExecutable.Create(async (int id, CancellationToken token) =>
  {
    await Task.Delay(50, token);
    return $"User-{id}";
  })
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
