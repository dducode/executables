namespace Executables.Core.Executables;

internal sealed class FlattenExecutable<T1, T2>(IExecutable<T1, IExecutable<T1, T2>> inner) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  private readonly IExecutor<T1, IExecutable<T1, T2>> _inner = inner.GetExecutor();

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return _inner.Execute(input).GetExecutor().Execute(input);
  }

}