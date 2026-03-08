namespace Interactions.Executables;

internal sealed class TupleAggregator<T1, T2> : IAggregator<T1, T2, (T1, T2)> {

  internal static TupleAggregator<T1, T2> Instance { get; } = new();

  private TupleAggregator() { }

  public (T1, T2) Aggregate(T1 first, T2 second) {
    return (first, second);
  }

}