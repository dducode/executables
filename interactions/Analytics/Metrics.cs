namespace Interactions.Analytics;

public interface IMetrics<in T1, in T2> {

  void Call(string tag, T1 input);
  void Success(string tag, T2 output);
  void Failure(string tag, Exception exception);
  void Latency(string tag, TimeSpan duration);

}