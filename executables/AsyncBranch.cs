using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating asynchronous conditional executor branches.
/// </summary>
public static class AsyncBranch {

  /// <summary>
  /// Creates an asynchronous executor that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IAsyncExecutor<T1, T2> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new AsyncWhenExecutor<T1, T2>(condition, executor);
  }

  /// <summary>
  /// Creates an asynchronous executor that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IAsyncExecutor<T1, Optional<T2>> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new AsyncWhenFlattenExecutor<T1, T2>(condition, executor);
  }

  /// <summary>
  /// Creates a parameterless asynchronous executor that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IAsyncExecutor<Unit, T> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return When(_ => condition(), executor);
  }

  /// <summary>
  /// Creates a parameterless asynchronous executor that runs only when the condition is satisfied and flattens an optional result.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executor">Asynchronous executor to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns the inner optional result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executor"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IAsyncExecutor<Unit, Optional<T>> executor) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return When(_ => condition(), executor);
  }

  /// <summary>
  /// Creates an asynchronous executor from an asynchronous executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<T1, Optional<T2>> When<T1, T2>(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return When(condition, executable.GetExecutor());
  }

  /// <summary>
  /// Creates a parameterless asynchronous executor from an asynchronous executable that runs only when the condition is satisfied.
  /// </summary>
  /// <param name="condition">Condition that decides whether execution should proceed.</param>
  /// <param name="executable">Asynchronous executable to run when the condition is satisfied.</param>
  /// <returns>Asynchronous executor that returns an <see cref="Optional{T}"/> containing the result when executed.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IAsyncExecutor<Unit, Optional<T>> When<T>(Func<bool> condition, IAsyncExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return When(condition, executable.GetExecutor());
  }

}
