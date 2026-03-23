using Interactions.Core;

namespace Interactions;

/// <summary>
/// Represents a synchronous operator that can transform both input and output around another executor.
/// </summary>
/// <typeparam name="T1">Input type accepted by the operator.</typeparam>
/// <typeparam name="T2">Input type expected by the wrapped executor.</typeparam>
/// <typeparam name="T3">Output type returned by the wrapped executor.</typeparam>
/// <typeparam name="T4">Output type returned by the operator.</typeparam>
public abstract class ExecutionOperator<T1, T2, T3, T4> {

  /// <summary>
  /// Invokes the operator and optionally delegates execution to the supplied executor.
  /// </summary>
  /// <param name="input">Input value for the operator.</param>
  /// <param name="executor">Wrapped executor that can be invoked by the operator.</param>
  /// <returns>Result produced by the operator.</returns>
  public abstract T4 Invoke(T1 input, IExecutor<T2, T3> executor);

}