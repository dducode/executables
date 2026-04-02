namespace Executables.Core.Executables;

internal sealed class OrElseExecutable<T1, T2>(IExecutable<T1, Optional<T2>> inner, IExecutable<T1, T2> other) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  private readonly IExecutor<T1, Optional<T2>> _inner = inner.GetExecutor();
  private readonly IExecutor<T1, T2> _other = other.GetExecutor();

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    Optional<T2> result = _inner.Execute(input);
    return result.HasValue ? result.Value : _other.Execute(input);
  }

}