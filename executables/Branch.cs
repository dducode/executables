using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating conditional executors.
/// </summary>
public static class Branch {

  /// <summary>
  /// Creates an executor that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IExecutor<T1, T2> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new WhenExecutor<T1, T2>(condition, executor);
  }

  /// <summary>
  /// Creates an executor that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IExecutor<T1, Optional<T2>> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new WhenFlattenExecutor<T1, T2>(condition, executor);
  }

  /// <summary>
  /// Creates a parameterless executor that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IExecutor<Unit, T> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return When(_ => condition(), executor);
  }

  /// <summary>
  /// Creates a parameterless executor that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Executor to run when the condition is satisfied.</param>
  /// <returns>Executor that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IExecutor<Unit, Optional<T>> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return When(_ => condition(), executor);
  }

  /// <summary>
  /// Creates an executor from an executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return When(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Creates a parameterless executor from an executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Executable to run when the condition is satisfied.</param>
  /// <returns>Executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return When(condition, executable.GetExecutor());
  }

}