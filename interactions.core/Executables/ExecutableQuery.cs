namespace Interactions.Core.Executables;

internal sealed class ExecutableQuery<T1, T2>(IExecutable<T1, T2> inner) : IQuery<T1, T2>, IExecutor<T1, T2> {

  private readonly IExecutor<T1, T2> _inner = inner.GetExecutor();

  public T2 Send(T1 input) {
    return _inner.Execute(input);
  }

  public IExecutor<T1, T2> GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return Send(input);
  }

}