using Executables.Core.Operators;

namespace Executables.Operations;

/// <summary>
/// Provides suppression operator for executables.
/// </summary>
public readonly ref struct SuppressExceptionOperatorProvider<T1, T2>(IExecutable<T1, T2> executable) {

  /// <summary>
  /// Suppresses exceptions of the specified type and converts the result into <see cref="Optional{T}" />.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Executable that returns an empty optional when suppressed exception occurs.</returns>
  public IExecutable<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return executable.Apply(SuppressExceptionOperator<T1, T2, TEx>.Instance);
  }

}