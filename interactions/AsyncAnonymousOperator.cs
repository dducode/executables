using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions;

internal sealed class AsyncAnonymousOperator<T1, T2, T3, T4>(AsyncExecutionFunc<T1, T2, T3, T4> operation) : AsyncExecutionOperator<T1, T2, T3, T4> {

  public override ValueTask<T4> Invoke(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default) {
    return operation(input, next, token);
  }

}