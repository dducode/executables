using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Maps;

public static partial class MapExtensions {

  [Pure]
  public static IExecutable<T1, T4> Apply<T1, T2, T3, T4>(this Map<T1, T2, T3, T4> map, IExecutable<T2, T3> executable) {
    return new ExecutableMap<T1, T2, T3, T4>(map, executable);
  }

}