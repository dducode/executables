namespace Executables.Core.Executors;

internal sealed class TransitiveExecutor<T1, T2>(IExecutor<T1, T2> executor, Action<T2> action) : IExecutor<T1, T2> {

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    T2 result = executor.Execute(input);
    action(result);
    return result;
  }

}