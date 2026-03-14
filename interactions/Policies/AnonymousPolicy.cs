using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AnonymousPolicy<T1, T2>(Func<T1, IExecutor<T1, T2>, T2> func) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    return func(input, executor);
  }

}