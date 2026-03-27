using System.Diagnostics.Contracts;

namespace Interactions.Linq;

public static class LinqExtensions {

  [Pure]
  public static IExecutable<T1, T3> Select<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> selector) {
    return executable.Then(selector);
  }

}