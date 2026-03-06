using Interactions.Core;

namespace Interactions.Policies;

internal sealed class DynamicPolicy<T1, T2>(IProvider<Policy<T1, T2>> provider) : Policy<T1, T2> {

  public override T2 Execute(T1 input, IExecutable<T1, T2> executable) {
    Policy<T1, T2> inner = provider.Get();
    return inner != null ? inner.Execute(input, executable) : throw new InvalidOperationException($"Cannot resolve policy by {provider.GetType().Name}");
  }

}

internal sealed class AsyncDynamicPolicy<T1, T2>(IProvider<AsyncPolicy<T1, T2>> provider) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Execute(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token) {
    AsyncPolicy<T1, T2> inner = provider.Get();
    return inner?.Execute(input, executable, token) ?? throw new InvalidOperationException($"Cannot resolve policy by {provider.GetType().Name}");
  }

}