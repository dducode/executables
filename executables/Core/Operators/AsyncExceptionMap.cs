using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class AsyncExceptionMap<T1, T2, TFrom>(Func<TFrom, Exception> map) : AsyncBehaviorOperator<T1, T2> where TFrom : Exception {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    try {
      return await executor.Execute(input, token);
    }
    catch (TFrom ex) {
      throw map(ex);
    }
  }

}