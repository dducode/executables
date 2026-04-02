using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static partial class AsyncExecutable {

  /// <summary>
  /// Creates an asynchronous executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncWhenExecutable<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Creates an asynchronous executable that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IAsyncExecutable<T1, Optional<T2>> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncWhenFlattenExecutable<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Creates a parameterless asynchronous executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, IAsyncExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncWhenExecutable<Unit, T>(_ => condition(), executable);
  }

  /// <summary>
  /// Creates a parameterless asynchronous executable that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, IAsyncExecutable<Unit, Optional<T>> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncWhenFlattenExecutable<Unit, T>(_ => condition(), executable);
  }

  /// <summary>
  /// Creates an asynchronous executable from a delegate that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="func">Asynchronous delegate to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, AsyncFunc<T1, T2> func) {
    return When(condition, Create(func));
  }

  /// <summary>
  /// Creates a parameterless asynchronous executable from a delegate that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="func">Asynchronous delegate to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, AsyncFunc<T> func) {
    return When(condition, Create(func));
  }

  /// <summary>
  /// Creates an asynchronous executable from an action that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="action">Asynchronous action to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an optional <see cref="Unit"/> when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Optional<Unit>> When<T>(Func<T, bool> condition, AsyncAction<T> action) {
    return When(condition, Create(action));
  }

  /// <summary>
  /// Creates a parameterless asynchronous executable from an action that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="action">Asynchronous action to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executable that returns an optional <see cref="Unit"/> when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Optional<Unit>> When(Func<bool> condition, AsyncAction action) {
    return When(condition, Create(action));
  }

}
