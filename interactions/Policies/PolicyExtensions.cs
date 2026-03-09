using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Policies;

public static partial class PolicyExtensions {

  [Pure]
  public static IExecutable<T1, T2> Apply<T1, T2>(this Policy<T1, T2> policy, IExecutable<T1, T2> executable) {
    return new ExecutablePolicy<T1, T2>(policy, executable);
  }

}