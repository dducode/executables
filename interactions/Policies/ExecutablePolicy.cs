using Interactions.Core;

namespace Interactions.Policies;

internal sealed class ExecutablePolicy<T1, T2>(Policy<T1, T2> policy, IExecutable<T1, T2> inner) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return policy.Invoke(input, inner);
  }

}