namespace Interactions.Executables;

public interface IAggregator<in T1, in T2, out T3> {

  T3 Aggregate(T1 first, T2 second);

}