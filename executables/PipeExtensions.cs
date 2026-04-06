using System.Diagnostics.Contracts;
using Executables.Core.Pipes;
using Executables.Internal;

namespace Executables;

public static class PipeExtensions {

  /// <summary>
  /// Composes two pipes so that <paramref name="second"/> runs first and <paramref name="first"/> runs after it.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed pipe.</typeparam>
  /// <typeparam name="T2">Intermediate type between composed pipes.</typeparam>
  /// <typeparam name="T3">Output type of the composed pipe.</typeparam>
  /// <param name="first">Pipe applied after <paramref name="second"/>.</param>
  /// <param name="second">Pipe applied before <paramref name="first"/>.</param>
  /// <returns>Composed pipe that transforms <typeparamref name="T1"/> into <typeparamref name="T3"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T3> Compose<T1, T2, T3>(this IPipe<T2, T3> first, IPipe<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositePipe<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Composes a pipe with a preprocessing function.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed pipe.</typeparam>
  /// <typeparam name="T2">Intermediate type consumed by <paramref name="first"/>.</typeparam>
  /// <typeparam name="T3">Output type of the composed pipe.</typeparam>
  /// <param name="first">Pipe applied after <paramref name="second"/>.</param>
  /// <param name="second">Function that converts input into <typeparamref name="T2"/>.</param>
  /// <returns>Composed pipe that transforms <typeparamref name="T1"/> into <typeparamref name="T3"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T3> Compose<T1, T2, T3>(this IPipe<T2, T3> first, Func<T1, T2> second) {
    return first.Compose(Pipe.Create(second));
  }

  /// <summary>
  /// Chains two pipes so that <paramref name="first"/> runs first and <paramref name="second"/> runs after it.
  /// </summary>
  /// <typeparam name="T1">Input type of the chained pipe.</typeparam>
  /// <typeparam name="T2">Intermediate type between chained pipes.</typeparam>
  /// <typeparam name="T3">Output type of the chained pipe.</typeparam>
  /// <param name="first">Pipe applied before <paramref name="second"/>.</param>
  /// <param name="second">Pipe applied after <paramref name="first"/>.</param>
  /// <returns>Chained pipe that transforms <typeparamref name="T1"/> into <typeparamref name="T3"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T3> Then<T1, T2, T3>(this IPipe<T1, T2> first, IPipe<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Compose(first);
  }

  /// <summary>
  /// Chains a pipe with a postprocessing function.
  /// </summary>
  /// <typeparam name="T1">Input type of the chained pipe.</typeparam>
  /// <typeparam name="T2">Intermediate type produced by <paramref name="first"/>.</typeparam>
  /// <typeparam name="T3">Output type of the chained pipe.</typeparam>
  /// <param name="first">Pipe applied before <paramref name="second"/>.</param>
  /// <param name="second">Function that converts <typeparamref name="T2"/> into <typeparamref name="T3"/>.</param>
  /// <returns>Chained pipe that transforms <typeparamref name="T1"/> into <typeparamref name="T3"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T3> Then<T1, T2, T3>(this IPipe<T1, T2> first, Func<T2, T3> second) {
    return first.Then(Pipe.Create(second));
  }

  /// <summary>
  /// Adapts both input and output types of a pipe using mapping functions.
  /// </summary>
  /// <typeparam name="T1">Input type of the resulting pipe.</typeparam>
  /// <typeparam name="T2">Input type accepted by <paramref name="pipe"/>.</typeparam>
  /// <typeparam name="T3">Output type produced by <paramref name="pipe"/>.</typeparam>
  /// <typeparam name="T4">Output type of the resulting pipe.</typeparam>
  /// <param name="pipe">Pipe to adapt.</param>
  /// <param name="incoming">Function that maps incoming input into <typeparamref name="T2"/>.</param>
  /// <param name="outgoing">Function that maps output into <typeparamref name="T4"/>.</param>
  /// <returns>Pipe that accepts <typeparamref name="T1"/> and produces <typeparamref name="T4"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T4> Map<T1, T2, T3, T4>(this IPipe<T2, T3> pipe, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return pipe.Compose(incoming).Then(outgoing);
  }

}
