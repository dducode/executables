namespace Executables.Core.Executors;

internal sealed class WhenFlattenExecutor<T1, T2>(Func<T1, bool> condition, IExecutor<T1, Optional<T2>> executor) : IExecutor<T1, Optional<T2>> {

  Optional<T2> IExecutor<T1, Optional<T2>>.Execute(T1 input) {
    return condition(input) ? executor.Execute(input) : Optional<T2>.None;
  }

}