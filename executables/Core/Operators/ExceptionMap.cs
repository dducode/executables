using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ExceptionMap<T1, T2, TFrom>(Func<TFrom, Exception> map) : BehaviorOperator<T1, T2> where TFrom : Exception {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return executor.Execute(input);
    }
    catch (TFrom ex) {
      throw map(ex);
    }
  }

}