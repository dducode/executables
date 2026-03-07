using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AsyncDynamicPolicy<T1, T2>(IProvider<AsyncPolicy<T1, T2>> provider) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    AsyncPolicy<T1, T2> inner = provider.Get();
    return inner?.Invoke(input, executable, token) ?? throw new InvalidOperationException($"Cannot resolve policy by {provider.GetType().Name}");
  }

}