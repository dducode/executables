namespace Interactions;

/// <summary>
/// Represents an executor that processes input and returns a result.
/// </summary>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IExecutor<in TIn, out TOut> {

  /// <summary>
  /// Executes the operation for the specified input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <returns>Execution result.</returns>
  TOut Execute(TIn input);

}