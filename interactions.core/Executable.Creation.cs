using System.Diagnostics.Contracts;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions.Core;

public static class Executable {

  /// <summary>
  /// Returns an executable that returns its input unchanged.
  /// </summary>
  /// <typeparam name="T">Input and output type.</typeparam>
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
  /// Creates an executable from a function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="func">Function used for execution.</param>
  [Pure]
  public static IExecutable<T1, T2> Create<T1, T2>(Func<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T1, T2>(func);
  }

  /// <summary>
  /// Creates an executable from a parameterless function.
  /// </summary>
  /// <typeparam name="T">Output type.</typeparam>
  /// <param name="func">Function used for execution.</param>
  [Pure]
  public static IExecutable<Unit, T> Create<T>(Func<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableFunc<T>(func);
  }

  /// <summary>
  /// Creates an executable from an action.
  /// </summary>
  /// <typeparam name="T">Input type.</typeparam>
  /// <param name="action">Action used for execution.</param>
  [Pure]
  public static IExecutable<T> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction<T>(action);
  }

  /// <summary>
  /// Creates an executable from a parameterless action.
  /// </summary>
  /// <param name="action">Action used for execution.</param>
  [Pure]
  public static IExecutable<Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAction(action);
  }

}