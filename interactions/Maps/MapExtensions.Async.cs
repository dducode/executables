using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Maps;

public static partial class MapExtensions {

  [Pure]
  public static IAsyncExecutable<T1, T4> Apply<T1, T2, T3, T4>(this AsyncMap<T1, T2, T3, T4> map, IAsyncExecutable<T2, T3> executable) {
    return new AsyncExecutableMap<T1, T2, T3, T4>(map, executable);
  }

}