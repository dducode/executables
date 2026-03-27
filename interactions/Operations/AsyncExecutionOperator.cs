namespace Interactions.Operations;

/// <summary>
/// Represents an asynchronous operator that can transform both input and output around another executor.
/// </summary>
/// <typeparam name="T1">Input type accepted by the operator.</typeparam>
/// <typeparam name="T2">Input type expected by the wrapped executor.</typeparam>
/// <typeparam name="T3">Output type returned by the wrapped executor.</typeparam>
/// <typeparam name="T4">Output type returned by the operator.</typeparam>
public abstract class AsyncExecutionOperator<T1, T2, T3, T4> {

  /// <summary>
  /// Invokes the operator and optionally delegates execution to the supplied asynchronous executor.
  /// </summary>
  /// <param name="input">Input value for the operator.</param>
  /// <param name="executor">Wrapped executor that can be invoked by the operator.</param>
  /// <param name="token">Cancellation token for the asynchronous operation.</param>
  /// <returns>Result produced by the operator.</returns>
  public abstract ValueTask<T4> Invoke(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default);

}