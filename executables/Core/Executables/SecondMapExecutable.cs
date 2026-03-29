namespace Executables.Core.Executables;

internal sealed class SecondMapExecutable<T1, T2, TNew>(IExecutable<T2, TNew> map) : IExecutable<(T1, T2), (T1, TNew)>, IExecutor<(T1, T2), (T1, TNew)> {

  private readonly IExecutor<T2, TNew> _map = map.GetExecutor();

  IExecutor<(T1, T2), (T1, TNew)> IExecutable<(T1, T2), (T1, TNew)>.GetExecutor() {
    return this;
  }

  (T1, TNew) IExecutor<(T1, T2), (T1, TNew)>.Execute((T1, T2) input) {
    return (input.Item1, _map.Execute(input.Item2));
  }

}