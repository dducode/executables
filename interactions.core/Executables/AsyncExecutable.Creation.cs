using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

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