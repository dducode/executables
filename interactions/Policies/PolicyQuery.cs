using Interactions.Core.Queries;

namespace Interactions.Policies;

internal sealed class PolicyQuery<T1, T2>(IQuery<T1, T2> inner, Policy<T1, T2> policy) : IQuery<T1, T2> {

  public T2 Send(T1 input) {
    return policy.Execute(input, inner.Send);
  }

}

internal sealed class AsyncPolicyQuery<T1, T2>(IAsyncQuery<T1, T2> inner, AsyncPolicy<T1, T2> policy) : IAsyncQuery<T1, T2> {

  public ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    return policy.Execute(input, inner.Send, token);
  }

}