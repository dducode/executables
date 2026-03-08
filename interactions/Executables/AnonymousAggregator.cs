namespace Interactions.Executables;

internal sealed class AnonymousAggregator<T1, T2, T3>(Func<T1, T2, T3> construction) : IAggregator<T1, T2, T3> {

  public T3 Aggregate(T1 first, T2 second) {
    return construction(first, second);
  }

}