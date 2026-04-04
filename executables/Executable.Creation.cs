using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating executables.
/// </summary>
public static partial class Executable {

  /// <summary>
  /// Returns an executable that returns its input unchanged.
  /// </summary>
  [Pure]
  public static IExecutable<T, T> Identity<T>() {
    return IdentityExecutable<T>.Instance;
  }

  /// <summary>
  /// Returns a parameterless executable that returns <see cref="Unit"/> unchanged.
  /// </summary>
  /// <returns>Identity executable for <see cref="Unit"/>.</returns>
  [Pure]
  public static IExecutable<Unit, Unit> Identity() {
    return IdentityExecutable<Unit>.Instance;
  }

  /// <summary>
  /// Flattens an executable that returns another executable into a single executable.
  /// </summary>
  /// <param name="executable">Executable that returns the next executable to run for the same input.</param>
  /// <returns>Executable that executes the returned executable and exposes its result directly.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executable"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> FlatMap<T1, T2>(IExecutable<T1, IExecutable<T1, T2>> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new FlattenExecutable<T1, T2>(executable);
  }

  /// <summary>
  /// Creates a flattened executable from a delegate that selects the next executable to run.
  /// </summary>
  /// <param name="func">Delegate that returns the next executable for the provided input.</param>
  /// <returns>Executable that executes the returned executable and exposes its result directly.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> FlatMap<T1, T2>(Func<T1, IExecutable<T1, T2>> func) {
    return FlatMap(Create(func));
  }

  /// <summary>
  /// Creates an executable that preserves its input and appends a computed value.
  /// </summary>
  /// <param name="func">Delegate that computes an additional value from the input.</param>
  /// <returns>Executable that returns both the original input and the computed value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T1, T2)> Accumulate<T1, T2>(Func<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return Create((T1 x) => (x, func(x)));
  }

  /// <summary>
  /// Creates an executable that runs two executables for the same input and returns both results.
  /// </summary>
  /// <param name="first">First executable branch.</param>
  /// <param name="second">Second executable branch.</param>
  /// <returns>Executable that returns the results of both branches as a tuple.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(IExecutable<T1, T2> first, IExecutable<T1, T3> second) {
    ExceptionsHelper.ThrowIfNull(first, nameof(first));
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new ForkExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Creates an executable that runs two delegates for the same input and returns both results.
  /// </summary>
  /// <param name="first">First delegate branch.</param>
  /// <param name="second">Second delegate branch.</param>
  /// <returns>Executable that returns the results of both branches as a tuple.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(Func<T1, T2> first, Func<T1, T3> second) {
    return Fork(Create(first), Create(second));
  }

  /// <summary>
  /// Creates an executable from a function with four arguments.
  /// </summary>
  /// <param name="func">Function used for execution.</param>
  /// <returns>Executable that accepts a 4-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2, T3, T4), T5> Create<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T1, T2, T3, T4, T5>(func);
  }

  /// <summary>
  /// Creates an executable from a function with three arguments.
  /// </summary>
  /// <param name="func">Function used for execution.</param>
  /// <returns>Executable that accepts a 3-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2, T3), T4> Create<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T1, T2, T3, T4>(func);
  }

  /// <summary>
  /// Creates an executable from a function with two arguments.
  /// </summary>
  /// <param name="func">Function used for execution.</param>
  /// <returns>Executable that accepts a 2-tuple input and returns function result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2), T3> Create<T1, T2, T3>(Func<T1, T2, T3> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T1, T2, T3>(func);
  }

  /// <summary>
  /// Creates an executable from a function.
  /// </summary>
  /// <param name="func">Function used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Create<T1, T2>(Func<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T1, T2>(func);
  }

  /// <summary>
  /// Creates an executable from a parameterless function.
  /// </summary>
  /// <param name="func">Function used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, T> Create<T>(Func<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T>(func);
  }

  /// <summary>
  /// Creates an executable from an action with four arguments.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  /// <returns>Executable that accepts a 4-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2, T3, T4), Unit> Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction<T1, T2, T3, T4>(action);
  }

  /// <summary>
  /// Creates an executable from an action with three arguments.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  /// <returns>Executable that accepts a 3-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2, T3), Unit> Create<T1, T2, T3>(Action<T1, T2, T3> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction<T1, T2, T3>(action);
  }

  /// <summary>
  /// Creates an executable from an action with two arguments.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  /// <returns>Executable that accepts a 2-tuple input and returns <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<(T1, T2), Unit> Create<T1, T2>(Action<T1, T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction<T1, T2>(action);
  }

  /// <summary>
  /// Creates an executable from an action.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Unit> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction<T>(action);
  }

  /// <summary>
  /// Creates an executable from a parameterless action.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction(action);
  }

}