using System.Diagnostics.Contracts;
using Interactions.Core.Handleables;
using Interactions.Core.Internal;
using Interactions.Core.Lifecycle;

namespace Interactions.Core;

public static class Handleable {

  [Pure]
  public static IHandleable<T1, T2> Create<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AnonymousHandleable<T1, T2>(handling);
  }

  [Pure]
  public static IHandleable<T1, T2> FromEvent<T1, T2>(Action<Func<T1, T2>> add, Action<Func<T1, T2>> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<T1, T2> handler) => {
      add(handler.Handle);
      return Disposable.Create(() => remove(handler.Handle));
    });
  }

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