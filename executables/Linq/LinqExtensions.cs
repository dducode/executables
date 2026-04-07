using System.Diagnostics.Contracts;

namespace Executables.Linq;

/// <summary>
/// LINQ-style extension methods for executables.
/// </summary>
public static class LinqExtensions {

  /// <summary>
  /// Projects the result of an executable into a new form.
  /// </summary>
  /// <typeparam name="T1">Input type of the source executable.</typeparam>
  /// <typeparam name="T2">Result type of the source executable.</typeparam>
  /// <typeparam name="T3">Result type of the projected executable.</typeparam>
  /// <param name="executable">Source executable.</param>
  /// <param name="selector">Projection applied to the result value.</param>
  /// <returns>Executable that applies <paramref name="selector"/> to the source result.</returns>
  [Pure]
  public static IExecutable<T1, T3> Select<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> selector) {
    return executable.Then(selector);
  }

  /// <summary>
  /// Filters the result of an executable by converting it into an optional value.
  /// </summary>
  /// <typeparam name="T1">Input type of the source executable.</typeparam>
  /// <typeparam name="T2">Result type of the source executable.</typeparam>
  /// <param name="executable">Source executable.</param>
  /// <param name="predicate">Predicate that decides whether the result should be preserved.</param>
  /// <returns>Executable that returns the source result when <paramref name="predicate"/> succeeds; otherwise <see cref="Optional{T}.None"/>.</returns>
  public static IExecutable<T1, Optional<T2>> Where<T1, T2>(this IExecutable<T1, T2> executable, Func<T2, bool> predicate) {
    return executable.Then(t2 => predicate(t2) ? new Optional<T2>(t2) : Optional<T2>.None);
  }

  /// <summary>
  /// Filters the inner value of an optional executable result.
  /// </summary>
  /// <typeparam name="T1">Input type of the source executable.</typeparam>
  /// <typeparam name="T2">Inner result type of the optional value.</typeparam>
  /// <param name="executable">Source executable.</param>
  /// <param name="predicate">Predicate that decides whether the optional value should be preserved.</param>
  /// <returns>Executable that preserves the optional result only when it has a value and <paramref name="predicate"/> succeeds.</returns>
  public static IExecutable<T1, Optional<T2>> Where<T1, T2>(this IExecutable<T1, Optional<T2>> executable, Func<T2, bool> predicate) {
    return executable.Then(t2 => t2.HasValue && predicate(t2.Value) ? t2 : Optional<T2>.None);
  }

}