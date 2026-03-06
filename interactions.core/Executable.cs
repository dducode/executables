using System.Diagnostics.Contracts;

namespace Interactions.Core;

public interface IExecutable<in TIn, out TOut> {

  TOut Execute(TIn input);

}

public static class Executable {

  [Pure]
  public static IExecutable<T, T> Identity<T>() {
    return IdentityExecutable<T>.Instance;
  }

  [Pure]
  public static IExecutable<Unit, Unit> Identity() {
    return IdentityExecutable<Unit>.Instance;
  }

  [Pure]
  public static IExecutable<T1, T2> Create<T1, T2>(Func<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AnonymousExecutable<T1, T2>(func);
  }

  [Pure]
  public static IExecutable<Unit, T> Create<T>(Func<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AnonymousExecutable<Unit, T>(_ => func());
  }

  [Pure]
  public static IExecutable<T, Unit> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousExecutable<T, Unit>(input => {
      action(input);
      return default;
    });
  }

  [Pure]
  public static IExecutable<Unit, Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousExecutable<Unit, Unit>(_ => {
      action();
      return default;
    });
  }

}

public static class AsyncExecutable {

  [Pure]
  public static IAsyncExecutable<T1, T2> Create<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AsyncAnonymousExecutable<T1, T2>(func);
  }

  [Pure]
  public static IAsyncExecutable<Unit, T> Create<T>(AsyncFunc<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AsyncAnonymousExecutable<Unit, T>((_, token) => func(token));
  }

  [Pure]
  public static IAsyncExecutable<T, Unit> Create<T>(AsyncAction<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncAnonymousExecutable<T, Unit>(async (input, token) => {
      await action(input, token);
      return default;
    });
  }

  [Pure]
  public static IAsyncExecutable<Unit, Unit> Create(AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncAnonymousExecutable<Unit, Unit>(async (_, token) => {
      await action(token);
      return default;
    });
  }

}