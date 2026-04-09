using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Extension methods for building conditional executors.
/// </summary>
public static class ConditionalExecutorExtensions {

  /// <summary>
  /// Adds a conditional fallback executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> OrWhen<T1, T2>(this IExecutor<T1, Optional<T2>> executor, Func<T1, bool> condition, IExecutor<T1, T2> other) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new OrWhenExecutor<T1, T2>(executor, condition, other);
  }

  /// <summary>
  /// Adds a conditional fallback executed when the source executor produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutor<T1, Optional<T2>> executor,
    Func<T1, bool> condition,
    IExecutor<T1, Optional<T2>> other) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new OrWhenFlattenExecutor<T1, T2>(executor, condition, other);
  }

  /// <summary>
  /// Adds a parameterless conditional fallback executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated before running fallback logic.</param>
  /// <param name="other">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> OrWhen<T>(this IExecutor<Unit, Optional<T>> executor, Func<bool> condition, IExecutor<Unit, T> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executor.OrWhen(_ => condition(), other);
  }

  /// <summary>
  /// Adds a parameterless conditional fallback executed when the source executor produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated before running fallback logic.</param>
  /// <param name="other">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> OrWhen<T>(this IExecutor<Unit, Optional<T>> executor, Func<bool> condition, IExecutor<Unit, Optional<T>> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executor.OrWhen(_ => condition(), other);
  }

  /// <summary>
  /// Adds a conditional fallback executable executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutor<T1, Optional<T2>> executor,
    Func<T1, bool> condition,
    IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return executor.OrWhen(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Adds a parameterless conditional fallback executable executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated before running fallback logic.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> OrWhen<T>(this IExecutor<Unit, Optional<T>> executor, Func<bool> condition, IExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return executor.OrWhen(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Supplies a fallback executor when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="fallback">Fallback executor.</param>
  /// <returns>Executor producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> OrElse<T1, T2>(this IExecutor<T1, Optional<T2>> executor, IExecutor<T1, T2> fallback) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new OrElseExecutor<T1, T2>(executor, fallback);
  }

  /// <summary>
  /// Supplies a fallback executable when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="fallback">Fallback executable.</param>
  /// <returns>Executor producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> OrElse<T1, T2>(this IExecutor<T1, Optional<T2>> executor, IExecutable<T1, T2> fallback) {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return executor.OrElse(fallback.GetExecutor());
  }

}