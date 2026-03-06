using Interactions.Core;

namespace Interactions.Policies;

internal sealed class LazyPolicy<T1, T2>(IResolver<Policy<T1, T2>> resolver) : Policy<T1, T2> {

  private readonly Core.Lazy<Policy<T1, T2>> _inner = new(resolver);

  public override T2 Execute(T1 input, IExecutable<T1, T2> executable) {
    return _inner.Value.Execute(input, executable);
  }

}

internal sealed class AsyncLazyPolicy<T1, T2>(IResolver<AsyncPolicy<T1, T2>> resolver) : AsyncPolicy<T1, T2> {

  private readonly Core.Lazy<AsyncPolicy<T1, T2>> _inner = new(resolver);

  public override ValueTask<T2> Execute(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token) {
    return _inner.Value.Execute(input, executable, token);
  }

}