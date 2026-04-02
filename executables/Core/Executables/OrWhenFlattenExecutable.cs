namespace Executables.Core.Executables;

internal sealed class OrWhenFlattenExecutable<T1, T2>(Func<T1, bool> condition, IExecutable<T1, Optional<T2>> inner, IExecutable<T1, Optional<T2>> other)
  : IExecutable<T1, Optional<T2>>, IExecutor<T1, Optional<T2>> {

  private readonly IExecutor<T1, Optional<T2>> _inner = inner.GetExecutor();
  private readonly IExecutor<T1, Optional<T2>> _other = other.GetExecutor();

  IExecutor<T1, Optional<T2>> IExecutable<T1, Optional<T2>>.GetExecutor() {
    return this;
  }

  Optional<T2> IExecutor<T1, Optional<T2>>.Execute(T1 input) {
    Optional<T2> result = _inner.Execute(input);
    return result.HasValue ? result : condition(input) ? _other.Execute(input) : Optional<T2>.None;
  }

}