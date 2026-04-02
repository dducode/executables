using Executables.Core.Operators;

namespace Executables.Operations;

/// <summary>
/// Provides suppression operator for executables that already return <see cref="Optional{T}" />.
/// </summary>
public readonly ref struct SuppressExceptionFlattenOperatorProvider<T1, T2>(IExecutable<T1, Optional<T2>> executable) {

  /// <summary>
  /// Suppresses exceptions of the specified type and preserves optional result shape.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Executable that returns an empty optional when suppressed exception occurs.</returns>
  public IExecutable<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return executable.Apply(SuppressExceptionFlattenOperator<T1, T2, TEx>.Instance);
  }

}