using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions;

public abstract class AsyncExecutionOperator<T1, T2, T3, T4> {

  public abstract ValueTask<T4> Invoke(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default);

}

public static class AsyncExecutionOperator {

  [Pure]
  public static AsyncExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(AsyncExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AsyncAnonymousOperator<T1, T2, T3, T4>(operation);
  }

}