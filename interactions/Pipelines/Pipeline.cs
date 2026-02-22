using Interactions.Core;

namespace Interactions.Pipelines;

/// <summary>
/// Represents one middleware step in a synchronous pipeline.
/// </summary>
/// <typeparam name="T1">Input type accepted by the current step.</typeparam>
/// <typeparam name="T2">Input type expected by the downstream handler.</typeparam>
/// <typeparam name="T3">Output type returned by the downstream handler.</typeparam>
/// <typeparam name="T4">Output type returned by the current step.</typeparam>
public abstract class Pipeline<T1, T2, T3, T4> {

  /// <summary>
  /// Processes the current input and optionally delegates to <paramref name="next" />.
  /// </summary>
  /// <param name="input">Value handled by this middleware step.</param>
  /// <param name="next">Downstream handler that can be called zero or more times.</param>
  /// <returns>Result produced by the current step.</returns>
  public abstract T4 Invoke(T1 input, Handler<T2, T3> next);

}
