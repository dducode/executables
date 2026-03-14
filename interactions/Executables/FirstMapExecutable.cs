using Interactions.Core;

namespace Interactions.Executables;

internal sealed class FirstMapExecutable<T1, T2, TNew>(IExecutable<T1, TNew> map) : IExecutable<(T1, T2), (TNew, T2)>, IExecutor<(T1, T2), (TNew, T2)> {

  private readonly IExecutor<T1, TNew> _map = map.GetExecutor();

  IExecutor<(T1, T2), (TNew, T2)> IExecutable<(T1, T2), (TNew, T2)>.GetExecutor() {
    return this;
  }

  (TNew, T2) IExecutor<(T1, T2), (TNew, T2)>.Execute((T1, T2) input) {
    return (_map.Execute(input.Item1), input.Item2);
  }

}