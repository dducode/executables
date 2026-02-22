using Interactions.Core;

namespace Interactions.Pipelines;

/// <summary>
/// Represents one middleware step in an asynchronous pipeline.
/// </summary>
/// <typeparam name="T1">Input type accepted by the current step.</typeparam>
/// <typeparam name="T2">Input type expected by the downstream handler.</typeparam>
/// <typeparam name="T3">Output type returned by the downstream handler.</typeparam>
/// <typeparam name="T4">Output type returned by the current step.</typeparam>
public abstract class AsyncPipeline<T1, T2, T3, T4> {

  /// <summary>
  /// Processes the current input and optionally delegates to <paramref name="next" />.
  /// </summary>
  /// <param name="input">Value handled by this middleware step.</param>
  /// <param name="next">Downstream async handler that can be awaited zero or more times.</param>
  /// <param name="token">Cancellation token for cooperative cancellation.</param>
  /// <returns>Result produced by the current step.</returns>
  public abstract ValueTask<T4> Invoke(T1 input, AsyncHandler<T2, T3> next, CancellationToken token = default);

}
