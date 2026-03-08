using System.Diagnostics.Contracts;
using Interactions.Core.Internal;
using Interactions.Operations;

namespace Interactions;

public static class ExecutionOperator {

  [Pure]
  public static ExecutionOperator<T1, T1, T2, T2> Identity<T1, T2>() {
    return IdentityOperator<T1, T2>.Instance;
  }

  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(ExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}