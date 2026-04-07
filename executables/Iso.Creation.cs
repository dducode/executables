using System.Diagnostics.Contracts;
using Executables.Core.Iso;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating reversible transformations.
/// </summary>
public static class Iso {

  /// <summary>
  /// Returns an isomorphism that leaves values unchanged.
  /// </summary>
  /// <typeparam name="T">Value type.</typeparam>
  /// <returns>Identity isomorphism.</returns>
  [Pure]
  public static IIso<T, T> Identity<T>() {
    return IdentityIso<T>.Instance;
  }

  /// <summary>
  /// Creates an isomorphism from forward and backward conversion functions.
  /// </summary>
  /// <typeparam name="T1">Source type of the forward transformation.</typeparam>
  /// <typeparam name="T2">Source type of the backward transformation.</typeparam>
  /// <param name="forward">Function that converts <typeparamref name="T1"/> to <typeparamref name="T2"/>.</param>
  /// <param name="backward">Function that converts <typeparamref name="T2"/> to <typeparamref name="T1"/>.</param>
  /// <returns>Isomorphism that delegates conversions to the provided functions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="forward"/> or <paramref name="backward"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IIso<T1, T2> Create<T1, T2>(Func<T1, T2> forward, Func<T2, T1> backward) {
    ExceptionsHelper.ThrowIfNull(forward, nameof(forward));
    ExceptionsHelper.ThrowIfNull(backward, nameof(backward));
    return new AnonymousIso<T1, T2>(forward, backward);
  }

}