using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating asynchronous executables.
/// </summary>
public static partial class AsyncExecutable {

  /// <summary>
  /// Returns an asynchronous executable that returns its input unchanged.
  /// </summary>
  /// <returns>Asynchronous identity executable.</returns>
  [Pure]
  public static IAsyncExecutable<T, T> Identity<T>() {
    return AsyncIdentityExecutable<T>.Instance;
  }

  /// <summary>
  /// Returns a parameterless asynchronous executable that returns <see cref="Unit"/> unchanged.
  /// </summary>
  /// <returns>Asynchronous identity executable for <see cref="Unit"/>.</returns>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Identity() {
    return AsyncIdentityExecutable<Unit>.Instance;
  }

  /// <summary>
  /// Flattens an asynchronous executable that returns another asynchronous executable into a single executable.
  /// </summary>
  /// <param name="executable">Executable that returns the next executable to run for the same input.</param>
  /// <returns>Asynchronous executable that executes the returned executable and exposes its result directly.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executable"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> FlatMap<T1, T2>(IAsyncExecutable<T1, IAsyncExecutable<T1, T2>> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncFlattenExecutable<T1, T2>(executable);
  }

  /// <summary>
  /// Creates a flattened asynchronous executable from an asynchronous delegate that selects the next executable to run.
  /// </summary>
  /// <param name="func">Asynchronous delegate that returns the next executable for the provided input.</param>
  /// <returns>Asynchronous executable that executes the returned executable and exposes its result directly.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> FlatMap<T1, T2>(AsyncFunc<T1, IAsyncExecutable<T1, T2>> func) {
    return FlatMap(Create(func));
  }

  /// <summary>
  /// Creates a flattened asynchronous executable from a delegate that selects the next executable to run.
  /// </summary>
  /// <param name="func">Delegate that returns the next executable for the provided input.</param>
  /// <returns>Asynchronous executable that executes the returned executable and exposes its result directly.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> FlatMap<T1, T2>(Func<T1, IAsyncExecutable<T1, T2>> func) {
    return FlatMap(Executable.Create(func).ToAsyncExecutable());
  }

  /// <summary>
  /// Creates an asynchronous executable that preserves its input and appends a computed value.
  /// </summary>
  /// <param name="func">Asynchronous delegate that computes an additional value from the input.</param>
  /// <returns>Asynchronous executable that returns both the original input and the computed value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T1, T2)> Accumulate<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return Create(async (T1 x, CancellationToken token) => (x, await func(x, token)));
  }

  /// <summary>
  /// Creates an asynchronous executable that runs two asynchronous executables for the same input and returns both results.
  /// </summary>
  /// <param name="first">First executable branch.</param>
  /// <param name="second">Second executable branch.</param>
  /// <returns>Asynchronous executable that returns the results of both branches as a tuple.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T1, T3> second) {
    ExceptionsHelper.ThrowIfNull(first, nameof(first));
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncForkExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Creates an asynchronous executable that runs two asynchronous delegates for the same input and returns both results.
  /// </summary>
  /// <param name="first">First asynchronous delegate branch.</param>
  /// <param name="second">Second asynchronous delegate branch.</param>
  /// <returns>Asynchronous executable that returns the results of both branches as a tuple.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(AsyncFunc<T1, T2> first, AsyncFunc<T1, T3> second) {
    return Fork(Create(first), Create(second));
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