using System.Diagnostics.Contracts;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions;

public abstract class ExecutionOperator<T1, T2, T3, T4> {

  public abstract T4 Invoke(T1 input, IExecutable<T2, T3> next);

}

public static class ExecutionOperator {

  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(ExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}