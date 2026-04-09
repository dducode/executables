using System.Diagnostics.Contracts;
using Executables.Core.Iso;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Extension methods for composing and adapting isomorphisms.
/// </summary>
public static class IsoExtensions {

  /// <summary>
  /// Composes two isomorphisms so that <paramref name="second"/> runs first and <paramref name="first"/> runs after it.
  /// </summary>
  /// <typeparam name="T1">Source type of the composed isomorphism.</typeparam>
  /// <typeparam name="T2">Intermediate type between composed isomorphisms.</typeparam>
  /// <typeparam name="T3">Target type of the composed isomorphism.</typeparam>
  /// <param name="first">Isomorphism applied after <paramref name="second"/>.</param>
  /// <param name="second">Isomorphism applied before <paramref name="first"/>.</param>
  /// <returns>Composed isomorphism.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IIso<T1, T3> Compose<T1, T2, T3>(this IIso<T2, T3> first, IIso<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeIso<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Composes an isomorphism with forward and backward conversion functions.
  /// </summary>
  /// <typeparam name="T1">Source type of the composed isomorphism.</typeparam>
  /// <typeparam name="T2">Intermediate type consumed by <paramref name="first"/>.</typeparam>
  /// <typeparam name="T3">Target type of the composed isomorphism.</typeparam>
  /// <param name="first">Isomorphism applied after the provided conversion functions.</param>
  /// <param name="forward">Forward conversion from <typeparamref name="T1"/> to <typeparamref name="T2"/>.</param>
  /// <param name="backward">Backward conversion from <typeparamref name="T2"/> to <typeparamref name="T1"/>.</param>
  /// <returns>Composed isomorphism.</returns>
  [Pure]
  public static IIso<T1, T3> Compose<T1, T2, T3>(this IIso<T2, T3> first, Func<T1, T2> forward, Func<T2, T1> backward) {
    return first.Compose(Iso.Create(forward, backward));
  }

  /// <summary>
  /// Chains two isomorphisms so that <paramref name="first"/> runs first and <paramref name="second"/> runs after it.
  /// </summary>
  /// <typeparam name="T1">Source type of the chained isomorphism.</typeparam>
  /// <typeparam name="T2">Intermediate type between chained isomorphisms.</typeparam>
  /// <typeparam name="T3">Target type of the chained isomorphism.</typeparam>
  /// <param name="first">Isomorphism applied before <paramref name="second"/>.</param>
  /// <param name="second">Isomorphism applied after <paramref name="first"/>.</param>
  /// <returns>Chained isomorphism.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IIso<T1, T3> Then<T1, T2, T3>(this IIso<T1, T2> first, IIso<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Compose(first);
  }

  /// <summary>
  /// Chains an isomorphism with forward and backward conversion functions.
  /// </summary>
  /// <typeparam name="T1">Source type of the chained isomorphism.</typeparam>
  /// <typeparam name="T2">Intermediate type produced by <paramref name="first"/>.</typeparam>
  /// <typeparam name="T3">Target type of the chained isomorphism.</typeparam>
  /// <param name="first">Isomorphism applied before the provided conversion functions.</param>
  /// <param name="forward">Forward conversion from <typeparamref name="T2"/> to <typeparamref name="T3"/>.</param>
  /// <param name="backward">Backward conversion from <typeparamref name="T3"/> to <typeparamref name="T2"/>.</param>
  /// <returns>Chained isomorphism.</returns>
  [Pure]
  public static IIso<T1, T3> Then<T1, T2, T3>(this IIso<T1, T2> first, Func<T2, T3> forward, Func<T3, T2> backward) {
    return first.Then(Iso.Create(forward, backward));
  }

  /// <summary>
  /// Inverts an isomorphism by swapping its forward and backward directions.
  /// </summary>
  /// <typeparam name="T1">Original source type.</typeparam>
  /// <typeparam name="T2">Original target type.</typeparam>
  /// <param name="iso">Isomorphism to invert.</param>
  /// <returns>Inverted isomorphism.</returns>
  [Pure]
  public static IIso<T2, T1> Inverse<T1, T2>(this IIso<T1, T2> iso) {
    return new InvertedIso<T1, T2>(iso);
  }

  /// <summary>
  /// Exposes the forward direction of an isomorphism as an executable.
  /// </summary>
  /// <typeparam name="T1">Input type of the executable.</typeparam>
  /// <typeparam name="T2">Output type of the executable.</typeparam>
  /// <param name="iso">Isomorphism whose forward direction should be exposed.</param>
  /// <returns>Executable that delegates to <see cref="IIso{T1, T2}.Forward(T1)"/>.</returns>
  [Pure]
  public static IExecutable<T1, T2> AsExecutable<T1, T2>(this IIso<T1, T2> iso) {
    return Executable.Create<T1, T2>(iso.Forward);
  }

}