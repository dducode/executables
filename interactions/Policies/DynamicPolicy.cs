using Interactions.Core;

namespace Interactions.Policies;

internal sealed class DynamicPolicy<T1, T2>(IProvider<Policy<T1, T2>> provider) : Policy<T1, T2> {

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    Policy<T1, T2> inner = provider.Get();
    return inner != null ? inner.Execute(input, invocation) : throw new InvalidOperationException($"Cannot resolve policy by {provider.GetType().Name}");
  }

}

internal sealed class AsyncDynamicPolicy<T1, T2>(IProvider<AsyncPolicy<T1, T2>> provider) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    AsyncPolicy<T1, T2> inner = provider.Get();
    return inner?.Execute(input, invocation, token) ?? throw new InvalidOperationException($"Cannot resolve policy by {provider.GetType().Name}");
  }

}