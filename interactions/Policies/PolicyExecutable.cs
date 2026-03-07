using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class PolicyExecutable<T1, T2>(IExecutable<T1, T2> inner, Policy<T1, T2> policy) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return policy.Invoke(input, inner);
  }

}

internal sealed class AsyncPolicyExecutable<T1, T2>(IAsyncExecutable<T1, T2> inner, AsyncPolicy<T1, T2> policy) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    return policy.Invoke(input, inner, token);
  }

}