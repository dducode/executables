using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Guards;

public static class GuardExtensions {

  /// <summary>
  /// Composes two guards with logical AND semantics.
  /// Access is granted only if both guards grant access.
  /// </summary>
  /// <param name="first">First guard in composition chain.</param>
  /// <param name="second">Second guard in composition chain.</param>
  /// <returns>A composite guard that combines both checks.</returns>
  [Pure]
  public static Guard Compose(this Guard first, Guard second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeGuard(first, second);
  }

}