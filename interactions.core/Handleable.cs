using System.Diagnostics.Contracts;
using Interactions.Core.Extensions;

namespace Interactions.Core;

public abstract class Handleable<T1, T2> {

  public abstract IDisposable Handle(Handler<T1, T2> handler);

}

public abstract class AsyncHandleable<T1, T2> {

  public abstract IDisposable Handle(AsyncHandler<T1, T2> handler);

}

public static class Handleable {

  [Pure]
  public static Handleable<T1, T2> Create<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AnonymousHandleable<T1, T2>(handling);
  }

  [Pure]
  public static AsyncHandleable<T1, T2> Create<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AsyncAnonymousHandleable<T1, T2>(handling);
  }

  [Pure]
  public static Handleable<T1, T2> FromEvent<T1, T2>(Action<Func<T1, T2>> add, Action<Func<T1, T2>> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<T1, T2> handler) => {
      add(handler.Handle);
      return Disposable.Create(() => remove(handler.Handle));
    });
  }

  [Pure]
  public static Handleable<T, Unit> FromEvent<T>(Action<Action<T>> add, Action<Action<T>> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<T, Unit> handler) => {
      var action = new Action<T>(i => handler.Handle(i));
      add(action);
      return Disposable.Create(() => remove(action));
    });
  }

  [Pure]
  public static Handleable<Unit, Unit> FromEvent(Action<Action> add, Action<Action> remove) {
    ExceptionsHelper.ThrowIfNull(add, nameof(add));
    ExceptionsHelper.ThrowIfNull(remove, nameof(remove));

    return Create((Handler<Unit, Unit> handler) => {
      var action = new Action(() => handler.Handle(default));
      add(action);
      return Disposable.Create(() => remove(action));
    });
  }

  internal static IDisposable MergedHandle<T1, T2>(Handleable<T1, T2> first, Handleable<T1, T2> second, Handler<T1, T2> handler) {
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

  internal static IDisposable MergedHandle<T1, T2>(AsyncHandleable<T1, T2> first, AsyncHandleable<T1, T2> second, AsyncHandler<T1, T2> handler) {
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