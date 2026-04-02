using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static partial class Executable {

  /// <summary>
  /// Creates an executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new WhenExecutable<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Creates an executable that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IExecutable<T1, Optional<T2>> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new WhenFlattenExecutable<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Creates a parameterless executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, IExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new WhenExecutable<Unit, T>(_ => condition(), executable);
  }

  /// <summary>
  /// Creates a parameterless executable that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executable that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, IExecutable<Unit, Optional<T>> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new WhenFlattenExecutable<Unit, T>(_ => condition(), executable);
  }

  /// <summary>
  /// Creates an executable from a delegate that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="func">Delegate to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, Func<T1, T2> func) {
    return When(condition, Create(func));
  }

  /// <summary>
  /// Creates a parameterless executable from a delegate that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="func">Delegate to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, Optional<T>> When<T>(Func<bool> condition, Func<T> func) {
    return When(condition, Create(func));
  }

  /// <summary>
  /// Creates an executable from an action that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="action">Action to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an optional <see cref="Unit"/> when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Optional<Unit>> When<T>(Func<T, bool> condition, Action<T> action) {
    return When(condition, Create(action));
  }

  /// <summary>
  /// Creates a parameterless executable from an action that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="action">Action to run when the condition is satisfied.</param>
  /// <returns>Executable that returns an optional <see cref="Unit"/> when executed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, Optional<Unit>> When(Func<bool> condition, Action action) {
    return When(condition, Create(action));
  }

}
