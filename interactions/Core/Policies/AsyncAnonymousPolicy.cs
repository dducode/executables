using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class AsyncAnonymousPolicy<T1, T2>(AsyncFunc<T1, IAsyncExecutor<T1, T2>, T2> func) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    return func(input, executor, token);
  }

}