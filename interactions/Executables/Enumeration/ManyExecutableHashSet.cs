using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ManyExecutableHashSet<T1, T2>(IQuery<T1, HashSet<T2>> query, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyHashSetExecutor<T1, T2> GetEnumerator() {
    return new ManyHashSetExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}