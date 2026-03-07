using Interactions.Core;

namespace Interactions.Policies;

internal sealed class LazyPolicy<T1, T2>(IResolver<Policy<T1, T2>> resolver) : Policy<T1, T2> {

  private readonly Core.Internal.Lazy<Policy<T1, T2>> _inner = new(resolver);

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return _inner.Value.Invoke(input, executable);
  }

}