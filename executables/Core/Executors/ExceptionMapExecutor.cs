namespace Executables.Core.Executors;

internal sealed class ExceptionMapExecutor<T1, T2, TFrom>(IExecutor<T1, T2> executor, Func<TFrom, Exception> map) : IExecutor<T1, T2> where TFrom : Exception {

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    try {
      return executor.Execute(input);
    }
    catch (TFrom ex) {
      throw map(ex);
    }
  }

}