using Interactions.Core;
using Interactions.Guards;

namespace Interactions.Policies;

internal sealed class AsyncGuardPolicy<T1, T2>(Guard guard) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    if (guard.TryGetAccess())
      return executable.Execute(input, token);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}