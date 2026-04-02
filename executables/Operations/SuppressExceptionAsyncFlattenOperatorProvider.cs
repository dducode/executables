using Executables.Core.Operators;

namespace Executables.Operations;

/// <summary>
/// Provides suppression operator for asynchronous executables that already return <see cref="Optional{T}" />.
/// </summary>
public readonly ref struct SuppressExceptionAsyncFlattenOperatorProvider<T1, T2>(IAsyncExecutable<T1, Optional<T2>> executable) {

  /// <summary>
  /// Suppresses exceptions of the specified type and preserves optional result shape.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Asynchronous executable that returns an empty optional when suppressed exception occurs.</returns>
  public IAsyncExecutable<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return executable.Apply(SuppressExceptionAsyncFlattenOperator<T1, T2, TEx>.Instance);
  }

}