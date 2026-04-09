namespace Executables.Core.Executors;

internal sealed class SuppressExceptionExecutor<T1, T2, TEx>(IExecutor<T1, T2> executor) : IExecutor<T1, Optional<T2>> where TEx : Exception {

  Optional<T2> IExecutor<T1, Optional<T2>>.Execute(T1 input) {
    try {
      return new Optional<T2>(executor.Execute(input));
    }
    catch (TEx) {
      return Optional<T2>.None;
    }
  }

}