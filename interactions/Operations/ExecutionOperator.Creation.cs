using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Operations;

public static class ExecutionOperator {

  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(ExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}