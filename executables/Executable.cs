using System.Diagnostics.Contracts;

namespace Executables;

/// <summary>
/// Represents an executable operation.
/// </summary>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IExecutable<in TIn, out TOut> {

  /// <summary>
  /// Gets an executor that performs this executable operation.
  /// </summary>
  /// <returns>Executor instance.</returns>
  [Pure]
  IExecutor<TIn, TOut> GetExecutor();

}