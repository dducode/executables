using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AnonymousPolicy<T1, T2>(Func<T1, IExecutable<T1, T2>, T2> func) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> next) {
    return func(input, next);
  }

}