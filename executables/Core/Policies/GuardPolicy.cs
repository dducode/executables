using Executables.Guards;
using Executables.Policies;

namespace Executables.Core.Policies;

internal sealed class GuardPolicy<T1, T2>(Guard guard) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    if (guard.TryGetAccess())
      return executor.Execute(input);
    throw new AccessDeniedException(guard.ErrorMessage);
  }

}