using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;
using Executables.Lifecycle;

namespace Executables.Handling;

/// <summary>
/// Factory methods for creating handleables.
/// </summary>
public static class Handleable {

  /// <summary>
  /// Creates a handleable from a registration delegate.
  /// </summary>
  /// <param name="handling">Delegate that registers a handler and returns a handle for unregistering it.</param>
  /// <returns>Handleable backed by <paramref name="handling"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="handling"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IHandleable<T1, T2> Create<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AnonymousHandleable<T1, T2>(handling);
  }

  /// <summary>
  /// Creates a handleable from an event with add and remove accessors.
  /// </summary>
  /// <param name="add">Action that subscribes a delegate to the source event.</param>
  /// <param name="remove">Action that unsubscribes a delegate from the source event.</param>
  /// <returns>Handleable wrapping the event subscription lifecycle.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="add"/> or <paramref name="remove"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IHandleable<T1, T2> FromEvent<T1, T2>(Action<Func<T1, T2>> add, Action<Func<T1, T2>> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<T1, T2> handler) => {
      add(handler.Handle);
      return Disposable.Create(() => remove(handler.Handle));
    });
  }

  /// <summary>
  /// Creates a handleable from an event with typed action handlers.
  /// </summary>
  /// <param name="add">Action that subscribes a delegate to the source event.</param>
  /// <param name="remove">Action that unsubscribes a delegate from the source event.</param>
  /// <returns>Handleable wrapping the event subscription lifecycle.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="add"/> or <paramref name="remove"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IHandleable<T, Unit> FromEvent<T>(Action<Action<T>> add, Action<Action<T>> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<T, Unit> handler) => {
      var action = new Action<T>(i => handler.Handle(i));
      add(action);
      return Disposable.Create(() => remove(action));
    });
  }

  /// <summary>
  /// Creates a handleable from a parameterless event.
  /// </summary>
  /// <param name="add">Action that subscribes a delegate to the source event.</param>
  /// <param name="remove">Action that unsubscribes a delegate from the source event.</param>
  /// <returns>Handleable wrapping the event subscription lifecycle.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="add"/> or <paramref name="remove"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IHandleable<Unit, Unit> FromEvent(Action<Action> add, Action<Action> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<Unit, Unit> handler) => {
      var action = new Action(() => handler.Handle(default));
      add(action);
      return Disposable.Create(() => remove(action));
    });
  }

  internal static IDisposable MergedHandle<T1, T2, THandler>(
    IHandleable<T1, T2, THandler> first,
    IHandleable<T1, T2, THandler> second,
    THandler handler) where THandler : Handler<T1, T2> {
    IDisposable firstDisposable = null;
    IDisposable secondDisposable = null;

    try {
      firstDisposable = first.Handle(handler);
      secondDisposable = second.Handle(handler);
      return firstDisposable.Compose(secondDisposable);
    }
    catch (Exception) {
      if (firstDisposable == null)
        throw;

      if (secondDisposable == null)
        firstDisposable.Dispose();

      throw;
    }
  }

  internal static IDisposable MergedHandle<T1, T2, THandler>(
    IAsyncHandleable<T1, T2, THandler> first,
    IAsyncHandleable<T1, T2, THandler> second,
    THandler handler) where THandler : AsyncHandler<T1, T2> {
    IDisposable firstDisposable = null;
    IDisposable secondDisposable = null;

    try {
      firstDisposable = first.Handle(handler);
      secondDisposable = second.Handle(handler);
      return firstDisposable.Compose(secondDisposable);
    }
    catch (Exception) {
      if (firstDisposable == null)
        throw;

      if (secondDisposable == null)
        firstDisposable.Dispose();

      throw;
    }
  }

}