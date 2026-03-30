using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating executables.
/// </summary>
public static class Executable {

  /// <summary>
  /// Returns an executable that returns its input unchanged.
  /// </summary>
  [Pure]
  public static IExecutable<T, T> Identity<T>() {
    return IdentityExecutable<T>.Instance;
  }

  /// <inheritdoc cref="Identity{T}" />
  [Pure]
  public static IExecutable<Unit, Unit> Identity() {
    return IdentityExecutable<Unit>.Instance;
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