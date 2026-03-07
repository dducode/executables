using Interactions.Core;
using Interactions.Guards;

namespace Interactions.Policies;

internal sealed class GuardPolicy<T1, T2>(Guard guard) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    if (guard.TryGetAccess())
      return executable.Execute(input);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}