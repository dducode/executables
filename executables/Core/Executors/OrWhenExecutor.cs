namespace Executables.Core.Executors;

internal sealed class OrWhenExecutor<T1, T2>(IExecutor<T1, Optional<T2>> first, Func<T1, bool> condition, IExecutor<T1, T2> second)
  : IExecutor<T1, Optional<T2>> {

  Optional<T2> IExecutor<T1, Optional<T2>>.Execute(T1 input) {
    Optional<T2> result = first.Execute(input);
    return result.HasValue ? result : condition(input) ? new Optional<T2>(second.Execute(input)) : Optional<T2>.None;
  }

}