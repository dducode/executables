using System.Diagnostics.Contracts;
using Executables.Core.Guards;
using Executables.Internal;

namespace Executables.Guards;

/// <summary>
/// Factory methods for creating guards.
/// </summary>
public abstract partial class Guard {

  /// <summary>
  /// Creates a manually controlled guard.
  /// </summary>
  /// <param name="errorMessage">Message returned when guard denies access.</param>
  /// <returns>Guard that can deny and allow an access programmatically.</returns>
  [Pure]
  public static ToggleGuard Manual(string errorMessage) {
    ExceptionsHelper.ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage));
    return new ToggleGuard(errorMessage);
  }

  /// <summary>
  /// Creates a guard from a predicate and error message.
  /// </summary>
  /// <param name="condition">Predicate that returns true to allow access.</param>
  /// <param name="message">Error message used when access is denied.</param>
  /// <returns>Guard that evaluates <paramref name="condition"/> to decide whether access is allowed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="condition"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
  [Pure]
  public static Guard Create(Func<bool> condition, string message) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNullOrEmpty(message, nameof(message));
    return new AnonymousGuard(condition, message);
  }

}