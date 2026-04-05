namespace Executables.Core.Executors;

internal sealed class OrElseExecutor<T1, T2>(IExecutor<T1, Optional<T2>> first, IExecutor<T1, T2> second) : IExecutor<T1, T2> {

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    Optional<T2> result = first.Execute(input);
    return result.HasValue ? result.Value : second.Execute(input);
  }

}