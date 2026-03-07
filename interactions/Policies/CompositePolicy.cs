using Interactions.Core;
using Interactions.Operations;

namespace Interactions.Policies;

internal sealed class CompositePolicy<T1, T2>(Policy<T1, T2> first, Policy<T1, T2> second) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return second.Invoke(input, first.AsExecutable(executable));
  }

}