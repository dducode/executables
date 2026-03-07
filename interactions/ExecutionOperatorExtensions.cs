using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions;

public static class ExecutionOperatorExtensions {

  public static IExecutable<T1, T4> AsExecutable<T1, T2, T3, T4>(this ExecutionOperator<T1, T2, T3, T4> executionOperator, IExecutable<T2, T3> executable) {
    return Executable.Create<T1, T4>(i => executionOperator.Invoke(i, executable));
  }

  public static IAsyncExecutable<T1, T4> AsExecutable<T1, T2, T3, T4>(
    this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
    IAsyncExecutable<T2, T3> executable) {
    return AsyncExecutable.Create<T1, T4>((i, t) => executionOperator.Invoke(i, executable, t));
  }

}