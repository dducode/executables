using Interactions.Core;

namespace Interactions.Executables;

internal sealed class FlowExecutable<T1, T2, T3, T4>(
  IExecutable<T1, T2> first,
  IExecutable<T1, T3> second,
  IAggregator<T2, T3, T4> aggregator) : IExecutable<T1, T4> {

  public T4 Execute(T1 input) {
    return aggregator.Aggregate(first.Execute(input), second.Execute(input));
  }

}