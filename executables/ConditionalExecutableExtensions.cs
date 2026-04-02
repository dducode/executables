using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static class ConditionalExecutableExtensions {

  /// <summary>
  /// Adds a conditional fallback executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutable<T1, Optional<T2>> executable,
    Func<T1, bool> condition,
    IExecutable<T1, T2> other) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new OrWhenExecutable<T1, T2>(condition, executable, other);
  }

  /// <summary>
  /// Adds a conditional fallback executed when the source executable produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutable<T1, Optional<T2>> executable,
    Func<T1, bool> condition,
    IExecutable<T1, Optional<T2>> other) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new OrWhenFlattenExecutable<T1, T2>(condition, executable, other);
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutable<T1, Optional<T2>> executable,
    Func<T1, bool> condition,
    IAsyncExecutable<T1, T2> other) {
    return executable.ToAsyncExecutable().OrWhen(condition, other);
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback executed when the source executable produces no value and flattens an optional fallback result.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutable<T1, Optional<T2>> executable,
    Func<T1, bool> condition,
    IAsyncExecutable<T1, Optional<T2>> other) {
    return executable.ToAsyncExecutable().OrWhen(condition, other);
  }

  /// <summary>
  /// Adds a conditional fallback delegate executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Delegate to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> OrWhen<T1, T2>(this IExecutable<T1, Optional<T2>> executable, Func<T1, bool> condition, Func<T1, T2> other) {
    return executable.OrWhen(condition, Executable.Create(other));
  }

  /// <summary>
  /// Adds a parameterless conditional fallback delegate executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Delegate to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<Unit, Optional<T>> OrWhen<T>(this IExecutable<Unit, Optional<T>> executable, Func<bool> condition, Func<T> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executable.OrWhen(_ => condition(), Executable.Create(other));
  }

  /// <summary>
  /// Adds a conditional fallback action executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Action to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T, Optional<Unit>> OrWhen<T>(this IExecutable<T, Optional<Unit>> executable, Func<T, bool> condition, Action<T> other) {
    return executable.OrWhen(condition, Executable.Create(other));
  }

  /// <summary>
  /// Adds a parameterless conditional fallback action executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Action to run when the condition is satisfied.</param>
  /// <returns>Executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<Unit, Optional<Unit>> OrWhen(this IExecutable<Unit, Optional<Unit>> executable, Func<bool> condition, Action other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executable.OrWhen(_ => condition(), Executable.Create(other));
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback delegate executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous delegate to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> OrWhen<T1, T2>(
    this IExecutable<T1, Optional<T2>> executable,
    Func<T1, bool> condition,
    AsyncFunc<T1, T2> other) {
    return executable.OrWhen(condition, AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional fallback delegate executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous delegate to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<T>> OrWhen<T>(this IExecutable<Unit, Optional<T>> executable, Func<bool> condition, AsyncFunc<T> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executable.OrWhen(_ => condition(), AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Adds an asynchronous conditional fallback action executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous action to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T, Optional<Unit>> OrWhen<T>(this IExecutable<T, Optional<Unit>> executable, Func<T, bool> condition, AsyncAction<T> other) {
    return executable.OrWhen(condition, AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional fallback action executed when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="condition">Condition evaluated against the original input.</param>
  /// <param name="other">Asynchronous action to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable producing the original or fallback optional result.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="other"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<Unit>> OrWhen(this IExecutable<Unit, Optional<Unit>> executable, Func<bool> condition, AsyncAction other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return executable.OrWhen(_ => condition(), AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Supplies a fallback executable when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback executable.</param>
  /// <returns>Executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> OrElse<T1, T2>(this IExecutable<T1, Optional<T2>> executable, IExecutable<T1, T2> other) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new OrElseExecutable<T1, T2>(executable, other);
  }

  /// <summary>
  /// Supplies an asynchronous fallback executable when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback asynchronous executable.</param>
  /// <returns>Asynchronous executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> OrElse<T1, T2>(this IExecutable<T1, Optional<T2>> executable, IAsyncExecutable<T1, T2> other) {
    return executable.ToAsyncExecutable().OrElse(other);
  }

  /// <summary>
  /// Supplies a fallback delegate when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback delegate.</param>
  /// <returns>Executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> OrElse<T1, T2>(this IExecutable<T1, Optional<T2>> executable, Func<T1, T2> other) {
    return executable.OrElse(Executable.Create(other));
  }

  /// <summary>
  /// Supplies a parameterless fallback delegate when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback delegate.</param>
  /// <returns>Executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, T> OrElse<T>(this IExecutable<Unit, Optional<T>> executable, Func<T> other) {
    return executable.OrElse(Executable.Create(other));
  }

  /// <summary>
  /// Supplies a fallback action when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback action.</param>
  /// <returns>Executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Unit> OrElse<T>(this IExecutable<T, Optional<Unit>> executable, Action<T> other) {
    return executable.OrElse(Executable.Create(other));
  }

  /// <summary>
  /// Supplies a parameterless fallback action when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback action.</param>
  /// <returns>Executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, Unit> OrElse(this IExecutable<Unit, Optional<Unit>> executable, Action other) {
    return executable.OrElse(Executable.Create(other));
  }

  /// <summary>
  /// Supplies an asynchronous fallback delegate when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback asynchronous delegate.</param>
  /// <returns>Asynchronous executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> OrElse<T1, T2>(this IExecutable<T1, Optional<T2>> executable, AsyncFunc<T1, T2> other) {
    return executable.OrElse(AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Supplies a parameterless asynchronous fallback delegate when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback asynchronous delegate.</param>
  /// <returns>Asynchronous executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> OrElse<T>(this IExecutable<Unit, Optional<T>> executable, AsyncFunc<T> other) {
    return executable.OrElse(AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Supplies an asynchronous fallback action when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback asynchronous action.</param>
  /// <returns>Asynchronous executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> OrElse<T>(this IExecutable<T, Optional<Unit>> executable, AsyncAction<T> other) {
    return executable.OrElse(AsyncExecutable.Create(other));
  }

  /// <summary>
  /// Supplies a parameterless asynchronous fallback action when the source executable produces no value.
  /// </summary>
  /// <param name="executable">Source optional executable.</param>
  /// <param name="other">Fallback asynchronous action.</param>
  /// <returns>Asynchronous executable producing either the original value or the fallback result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> OrElse(this IExecutable<Unit, Optional<Unit>> executable, AsyncAction other) {
    return executable.OrElse(AsyncExecutable.Create(other));
  }

}