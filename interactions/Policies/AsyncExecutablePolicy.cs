using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AsyncExecutablePolicy<T1, T2>(AsyncPolicy<T1, T2> policy, IAsyncExecutable<T1, T2> inner) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    return policy.Invoke(input, inner, token);
  }

}