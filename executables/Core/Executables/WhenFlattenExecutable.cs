namespace Executables.Core.Executables;

internal sealed class WhenFlattenExecutable<T1, T2>(Func<T1, bool> condition, IExecutable<T1, Optional<T2>> inner)
  : IExecutable<T1, Optional<T2>>, IExecutor<T1, Optional<T2>> {

  private readonly IExecutor<T1, Optional<T2>> _inner = inner.GetExecutor();

  IExecutor<T1, Optional<T2>> IExecutable<T1, Optional<T2>>.GetExecutor() {
    return this;
  }

  Optional<T2> IExecutor<T1, Optional<T2>>.Execute(T1 input) {
    return condition(input) ? _inner.Execute(input) : Optional<T2>.None;
  }

}