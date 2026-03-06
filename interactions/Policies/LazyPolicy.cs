using Interactions.Core;

namespace Interactions.Policies;

internal sealed class LazyPolicy<T1, T2>(IResolver<Policy<T1, T2>> resolver) : Policy<T1, T2> {

  private readonly Core.Lazy<Policy<T1, T2>> _inner = new(resolver);

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return _inner.Value.Invoke(input, executable);
  }

}

internal sealed class AsyncLazyPolicy<T1, T2>(IResolver<AsyncPolicy<T1, T2>> resolver) : AsyncPolicy<T1, T2> {

  private readonly Core.Lazy<AsyncPolicy<T1, T2>> _inner = new(resolver);

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    return _inner.Value.Invoke(input, executable, token);
  }

}