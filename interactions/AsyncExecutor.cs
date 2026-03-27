namespace Interactions;

/// <summary>
/// Represents an asynchronous executor that processes input and returns a result.
/// </summary>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IAsyncExecutor<in TIn, TOut> {

  /// <summary>
  /// Executes the operation for the specified input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <param name="token">Cancellation token.</param>
  /// <returns>Asynchronous execution result.</returns>
  ValueTask<TOut> Execute(TIn input, CancellationToken token = default);

}