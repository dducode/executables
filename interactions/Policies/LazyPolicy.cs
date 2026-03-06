using Interactions.Core;

namespace Interactions.Policies;

internal sealed class LazyPolicy<T1, T2>(IResolver<Policy<T1, T2>> resolver) : Policy<T1, T2> {

  private readonly Core.Lazy<Policy<T1, T2>> _inner = new(resolver);

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    return _inner.Value.Execute(input, invocation);
  }

}

internal sealed class AsyncLazyPolicy<T1, T2>(IResolver<AsyncPolicy<T1, T2>> resolver) : AsyncPolicy<T1, T2> {

  private readonly Core.Lazy<AsyncPolicy<T1, T2>> _inner = new(resolver);

  public override ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    return _inner.Value.Execute(input, invocation, token);
  }

}