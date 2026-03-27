using System.Diagnostics.Contracts;

namespace Interactions;

/// <summary>
/// Represents an asynchronous executable operation.
/// </summary>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IAsyncExecutable<in TIn, TOut> {

  /// <summary>
  /// Gets an async executor that performs this executable operation.
  /// </summary>
  /// <returns>Asynchronous executor instance.</returns>
  [Pure]
  IAsyncExecutor<TIn, TOut> GetExecutor();

}