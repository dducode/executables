using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static class CompositeExecutableExtensions {

  /// <summary>
  /// Prepends an executable to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Executable invoked first.</param>
  /// <returns>Executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Compose<T1, T2, T3>(this IExecutable<T2, T3> first, IExecutable<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Prepends a delegate to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Delegate invoked first.</param>
  /// <returns>Executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Compose<T1, T2, T3>(this IExecutable<T2, T3> first, Func<T1, T2> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless delegate to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless delegate invoked first.</param>
  /// <returns>Executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, T2> Compose<T1, T2>(this IExecutable<T1, T2> first, Func<T1> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends an action to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Action invoked first.</param>
  /// <returns>Executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Compose<T1, T2>(this IExecutable<Unit, T2> first, Action<T1> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless action to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless action invoked first.</param>
  /// <returns>Executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, T> Compose<T>(this IExecutable<Unit, T> first, Action second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends an asynchronous executable to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous executable invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IExecutable<T2, T3> first, IAsyncExecutable<T1, T2> second) {
    return first.ToAsyncExecutable().Compose(second);
  }

  /// <summary>
  /// Prepends an asynchronous delegate to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IExecutable<T2, T3> first, AsyncFunc<T1, T2> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless asynchronous delegate to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless asynchronous delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T2> Compose<T1, T2>(this IExecutable<T1, T2> first, AsyncFunc<T1> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends an asynchronous action to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Compose<T1, T2>(this IExecutable<Unit, T2> first, AsyncAction<T1> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless asynchronous action to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless asynchronous action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Compose<T>(this IExecutable<Unit, T> first, AsyncAction second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends an executable to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Compose(first);
  }

  /// <summary>
  /// Appends a delegate to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Delegate invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, Func<T2, T3> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless delegate to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless delegate invoked after <paramref name="first"/> completes.</param>
  /// <returns>Executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Then<T1, T2>(this IExecutable<T1, Unit> first, Func<T2> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends an action to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Action invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, Unit> Then<T1, T2>(this IExecutable<T1, T2> first, Action<T2> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless action to the current executable, creating a single synchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless action invoked after <paramref name="first"/> completes.</param>
  /// <returns>Executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Unit> Then<T>(this IExecutable<T, Unit> first, Action second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends an asynchronous executable to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Synchronous executable invoked first.</param>
  /// <param name="second">Asynchronous executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    return first.ToAsyncExecutable().Then(second);
  }

  /// <summary>
  /// Appends an asynchronous delegate to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Asynchronous delegate invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, AsyncFunc<T2, T3> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless asynchronous delegate to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless asynchronous delegate invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Then<T1, T2>(this IExecutable<T1, Unit> first, AsyncFunc<T2> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends an asynchronous action to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Asynchronous action invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> Then<T1, T2>(this IExecutable<T1, T2> first, AsyncAction<T2> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless asynchronous action to the current synchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless asynchronous action invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Then<T>(this IExecutable<T, Unit> first, AsyncAction second) {
    return first.Then(AsyncExecutable.Create(second));
  }

}