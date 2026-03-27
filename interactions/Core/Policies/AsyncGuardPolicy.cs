using Interactions.Guards;
using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class AsyncGuardPolicy<T1, T2>(Guard guard) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    if (guard.TryGetAccess())
      return executor.Execute(input, token);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}