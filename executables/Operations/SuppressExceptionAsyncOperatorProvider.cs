using Executables.Core.Operators;

namespace Executables.Operations;

/// <summary>
/// Provides suppression operator for asynchronous executables.
/// </summary>
public readonly ref struct SuppressExceptionAsyncOperatorProvider<T1, T2>(IAsyncExecutable<T1, T2> executable) {

  /// <summary>
  /// Suppresses exceptions of the specified type and converts the result into <see cref="Optional{T}" />.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Asynchronous executable that returns an empty optional when suppressed exception occurs.</returns>
  public IAsyncExecutable<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return executable.Apply(SuppressExceptionAsyncOperator<T1, T2, TEx>.Instance);
  }

}