using System.ComponentModel;
using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Executables;

namespace Interactions.Linq;

public static partial class LinqExecutableExtensions {

  [Pure, EditorBrowsable(EditorBrowsableState.Never)]
  public static IAsyncExecutable<T1, T3> Select<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> selector) {
    return executable.Then(selector);
  }

  [Pure, EditorBrowsable(EditorBrowsableState.Never)]
  public static IAsyncExecutable<T1, T4> SelectMany<T1, T2, T3, T4>(
    this IAsyncExecutable<T1, T2> executable,
    Func<T2, IAsyncExecutable<T2, T3>> selector,
    Func<T2, T3, T4> resultSelector) {
    return executable.Bind(Executable.Create((T2 x) => selector(x).Then(y => resultSelector(x, y))));
  }

  [Pure, EditorBrowsable(EditorBrowsableState.Never)]
  public static IAsyncExecutable<T1, T2> Where<T1, T2>(this IAsyncExecutable<T1, T2> executable, Func<T2, bool> predicate) {
    return executable.Then(x => predicate(x) ? x : throw new InvalidOperationException("Predicate failed"));
  }

}