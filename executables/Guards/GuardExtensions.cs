using System.Diagnostics.Contracts;
using Executables.Core.Guards;
using Executables.Internal;

namespace Executables.Guards;

/// <summary>
/// Extension methods for composing guards.
/// </summary>
public static class GuardExtensions {

  /// <summary>
  /// Composes two guards with logical AND semantics.
  /// Access is granted only if both guards grant access.
  /// </summary>
  /// <param name="first">First guard in composition chain.</param>
  /// <param name="second">Second guard in composition chain.</param>
  /// <returns>A composite guard that combines both checks.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static Guard Compose(this Guard first, Guard second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeGuard(first, second);
  }

}