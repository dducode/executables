using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Extension methods for building conditional asynchronous executors.
/// </summary>
public static class AsyncConditionalExecutorExtensions {

  /// <summary>
  /// Adds an asynchronous conditional fallback executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> OrWhen<T1, T2>(
    this IAsyncExecutor<T1, Optional<T2>> executor,
    Func<T1, bool> condition,
    IAsyncExecutor<T1, T2> other) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncOrWhenExecutor<T1, T2>(executor, condition, other);
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback executed when the source executor produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> OrWhen<T1, T2>(
    this IAsyncExecutor<T1, Optional<T2>> executor,
    Func<T1, bool> condition,
    IAsyncExecutor<T1, Optional<T2>> other) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncOrWhenFlattenExecutor<T1, T2>(executor, condition, other);
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional fallback executed when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> OrWhen<T>(
    this IAsyncExecutor<Unit, Optional<T>> executor,
    Func<bool> condition,
    IAsyncExecutor<Unit, T> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executor.OrWhen(_ => condition(), other);
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional fallback executed when the source executor produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> OrWhen<T>(
    this IAsyncExecutor<Unit, Optional<T>> executor,
    Func<bool> condition,
    IAsyncExecutor<Unit, Optional<T>> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executor.OrWhen(_ => condition(), other);
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback executed with an asynchronous executable when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> OrWhen<T1, T2>(
    this IAsyncExecutor<T1, Optional<T2>> executor,
    Func<T1, bool> condition,
    IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return executor.OrWhen(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional fallback executed with an asynchronous executable when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> OrWhen<T>(
    this IAsyncExecutor<Unit, Optional<T>> executor,
    Func<bool> condition,
    IAsyncExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return executor.OrWhen(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Supplies an asynchronous fallback executor when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="fallback">Fallback asynchronous executor.</param>
  /// <returns>Asynchronous executor producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> OrElse<T1, T2>(this IAsyncExecutor<T1, Optional<T2>> executor, IAsyncExecutor<T1, T2> fallback) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new AsyncOrElseExecutor<T1, T2>(executor, fallback);
  }

  /// <summary>
  /// Supplies an asynchronous fallback executable when the source executor produces no value.
  /// </summary>
  /// <param name="executor">Source optional executor.</param>
  /// <param name="fallback">Fallback asynchronous executable.</param>
  /// <returns>Asynchronous executor producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> OrElse<T1, T2>(this IAsyncExecutor<T1, Optional<T2>> executor, IAsyncExecutable<T1, T2> fallback) {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return executor.OrElse(fallback.GetExecutor());
  }

}