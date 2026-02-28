using Interactions.Core;
using Interactions.Guards;

namespace Interactions.Policies;

internal sealed class GuardPolicy<T1, T2>(Guard guard) : Policy<T1, T2> {

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    if (guard.TryGetAccess())
      return invocation.Invoke(input);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}

internal sealed class AsyncGuardPolicy<T1, T2>(Guard guard) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    if (guard.TryGetAccess())
      return invocation.Invoke(input, token);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}