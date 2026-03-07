using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

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