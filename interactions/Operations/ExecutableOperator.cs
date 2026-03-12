using Interactions.Core;

namespace Interactions.Operations;

internal sealed class ExecutableOperator<T1, T2, T3, T4>(
  ExecutionOperator<T1, T2, T3, T4> executionOperator,
  IExecutable<T2, T3> executable) : IExecutable<T1, T4> {

  public T4 Execute(T1 input) {
    return executionOperator.Invoke(input, executable);
  }

}

internal sealed class ExecutableOperator<T1, T2, T4>(
  ExecutionOperator<T1, T2, Unit, T4> executionOperator,
  IExecutable<T2> executable) : IExecutable<T1, T4> {

  private readonly IExecutable<T2, Unit> _wrapper = Executable.Create((T2 input) => {
    executable.Execute(input);
    return default(Unit);
  });

  public T4 Execute(T1 input) {
    return executionOperator.Invoke(input, _wrapper);
  }

}