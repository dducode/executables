using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AsyncAnonymousPolicy<T1, T2>(AsyncFunc<T1, IAsyncExecutable<T1, T2>, T2> func) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> next, CancellationToken token = default) {
    return func(input, next, token);
  }

}