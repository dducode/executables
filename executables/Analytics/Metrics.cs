namespace Executables.Analytics;

/// <summary>
/// Receives execution metrics for executables.
/// </summary>
public interface IMetrics<in T1, in T2> {

  /// <summary>
  /// Records the start of execution.
  /// </summary>
  /// <param name="tag">Optional execution tag.</param>
  /// <param name="input">Execution input.</param>
  void Call(string tag, T1 input);

  /// <summary>
  /// Records successful execution.
  /// </summary>
  /// <param name="tag">Optional execution tag.</param>
  /// <param name="output">Execution output.</param>
  void Success(string tag, T2 output);

  /// <summary>
  /// Records failed execution.
  /// </summary>
  /// <param name="tag">Optional execution tag.</param>
  /// <param name="exception">Exception thrown during execution.</param>
  void Failure(string tag, Exception exception);

  /// <summary>
  /// Records execution latency.
  /// </summary>
  /// <param name="tag">Optional execution tag.</param>
  /// <param name="duration">Measured execution duration.</param>
  void Latency(string tag, TimeSpan duration);

}