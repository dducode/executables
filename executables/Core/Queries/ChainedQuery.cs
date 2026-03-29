namespace Executables.Core.Queries;

internal sealed class ChainedQuery<T1, T2, T3>(IQuery<T1, T2> first, IQuery<T2, T3> second) : IQuery<T1, T3>, IExecutor<T1, T3> {

  public T3 Send(T1 input) {
    return second.Send(first.Send(input));
  }

  IExecutor<T1, T3> IExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  T3 IExecutor<T1, T3>.Execute(T1 input) {
    return Send(input);
  }

}