using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating asynchronous executables.
/// </summary>
public static class AsyncExecutable {

  /// <summary>
  /// Returns an async executable that returns its input unchanged.
  /// </summary>
  public static IAsyncExecutable<T, T> Identity<T>() {
    return AsyncIdentityExecutable<T>.Instance;
  }

  /// <inheritdoc cref="Identity{T}" />
  public static IAsyncExecutable<Unit, Unit> Identity() {
    return Identity<Unit>();
  }

  /// <summary>
  /// Creates an async executable from an async function with four arguments.
  /// </summary>
  /// <param name="func">Async function used for execution.</param>
  /// <returns>Async executable that accepts a 4-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2, T3, T4), T5> Create<T1, T2, T3, T4, T5>(AsyncFunc<T1, T2, T3, T4, T5> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T1, T2, T3, T4, T5>(func);
  }

  /// <summary>
  /// Creates an async executable from an async function with three arguments.
  /// </summary>
  /// <param name="func">Async function used for execution.</param>
  /// <returns>Async executable that accepts a 3-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2, T3), T4> Create<T1, T2, T3, T4>(AsyncFunc<T1, T2, T3, T4> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T1, T2, T3, T4>(func);
  }

  /// <summary>
  /// Creates an async executable from an async function with two arguments.
  /// </summary>
  /// <param name="func">Async function used for execution.</param>
  /// <returns>Async executable that accepts a 2-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2), T3> Create<T1, T2, T3>(AsyncFunc<T1, T2, T3> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T1, T2, T3>(func);
  }

  /// <summary>
  /// Creates an async executable from an async function.
  /// </summary>
  /// <param name="func">Async function used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Create<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T1, T2>(func);
  }

  /// <summary>
  /// Creates an async executable from a parameterless async function.
  /// </summary>
  /// <param name="func">Async function used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Create<T>(AsyncFunc<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T>(func);
  }

  /// <summary>
  /// Creates an async executable from an async action with four arguments.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  /// <returns>Async executable that accepts a 4-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2, T3, T4), Unit> Create<T1, T2, T3, T4>(AsyncAction<T1, T2, T3, T4> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction<T1, T2, T3, T4>(action);
  }

  /// <summary>
  /// Creates an async executable from an async action with three arguments.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  /// <returns>Async executable that accepts a 3-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2, T3), Unit> Create<T1, T2, T3>(AsyncAction<T1, T2, T3> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction<T1, T2, T3>(action);
  }

  /// <summary>
  /// Creates an async executable from an async action with two arguments.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  /// <returns>Async executable that accepts a 2-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<(T1, T2), Unit> Create<T1, T2>(AsyncAction<T1, T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction<T1, T2>(action);
  }

  /// <summary>
  /// Creates an async executable from an async action.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Create<T>(AsyncAction<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction<T>(action);
  }

  /// <summary>
  /// Creates an async executable from a parameterless async action.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Create(AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction(action);
  }

}