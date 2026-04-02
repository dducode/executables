using System.Diagnostics.Contracts;

namespace Executables.Linq;

public static class LinqExtensions {

  [Pure]
  public static IExecutable<T1, T3> Select<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> selector) {
    return executable.Then(selector);
  }

  public static IExecutable<T1, Optional<T2>> Where<T1, T2>(this IExecutable<T1, T2> executable, Func<T2, bool> predicate) {
    return executable.Then(t2 => predicate(t2) ? new Optional<T2>(t2) : Optional<T2>.None);
  }

  public static IExecutable<T1, Optional<T2>> Where<T1, T2>(this IExecutable<T1, Optional<T2>> executable, Func<T2, bool> predicate) {
    return executable.Then(t2 => t2.HasValue && predicate(t2.Value) ? t2 : Optional<T2>.None);
  }

}