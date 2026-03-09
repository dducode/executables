using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Policies;

public static partial class PolicyExtensions {

  [Pure]
  public static IAsyncExecutable<T1, T2> Apply<T1, T2>(this AsyncPolicy<T1, T2> policy, IAsyncExecutable<T1, T2> executable) {
    return new AsyncExecutablePolicy<T1, T2>(policy, executable);
  }

}