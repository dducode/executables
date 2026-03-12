using Interactions.Core;

namespace Interactions.Operations;

internal sealed class AsyncExecutableOperator<T1, T2, T3, T4>(
  AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
  IAsyncExecutable<T2, T3> executable) : IAsyncExecutable<T1, T4> {

  public ValueTask<T4> Execute(T1 input, CancellationToken token = default) {
    return executionOperator.Invoke(input, executable, token);
  }

}

internal sealed class AsyncExecutableOperator<T1, T2, T3>(
  AsyncExecutionOperator<T1, T2, Unit, T3> executionOperator,
  IAsyncExecutable<T2> executable) : IAsyncExecutable<T1, T3> {

  private readonly IAsyncExecutable<T2, Unit> _wrapper = AsyncExecutable.Create(async (T2 input, CancellationToken token) => {
    await executable.Execute(input, token);
    return default(Unit);
  });

  public ValueTask<T3> Execute(T1 input, CancellationToken token = default) {
    return executionOperator.Invoke(input, _wrapper, token);
  }

}