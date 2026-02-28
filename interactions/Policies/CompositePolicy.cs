using Interactions.Core;

namespace Interactions.Policies;

internal sealed class CompositePolicy<T1, T2>(Policy<T1, T2> first, Policy<T1, T2> second) : Policy<T1, T2> {

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    return second.Execute(input, i => first.Execute(i, invocation));
  }

}

internal sealed class AsyncCompositePolicy<T1, T2>(AsyncPolicy<T1, T2> first, AsyncPolicy<T1, T2> second) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    return second.Execute(input, (i, t) => first.Execute(i, invocation, t), token);
  }

}