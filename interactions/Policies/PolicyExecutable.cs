using Interactions.Core;

namespace Interactions.Policies;

internal sealed class PolicyExecutable<T1, T2>(IExecutable<T1, T2> inner, Policy<T1, T2> policy) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return policy.Invoke(input, inner);
  }

}