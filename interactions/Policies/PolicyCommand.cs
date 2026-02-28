using Interactions.Core.Commands;

namespace Interactions.Policies;

internal sealed class PolicyCommand<T>(ICommand<T> inner, Policy<T, bool> policy) : ICommand<T> {

  public bool Execute(T input) {
    return policy.Execute(input, inner.Execute);
  }

}

internal sealed class AsyncPolicyCommand<T>(IAsyncCommand<T> inner, AsyncPolicy<T, bool> policy) : IAsyncCommand<T> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
    return policy.Execute(input, inner.Execute, token);
  }

}