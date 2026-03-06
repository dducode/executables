using Interactions.Core;

namespace Interactions.Policies;

internal sealed class CompositePolicy<T1, T2>(Policy<T1, T2> first, Policy<T1, T2> second) : Policy<T1, T2> {

  public override T2 Execute(T1 input, IExecutable<T1, T2> executable) {
    return second.Execute(input, Executable.Create((T1 i) => first.Execute(i, executable)));
  }

}

internal sealed class AsyncCompositePolicy<T1, T2>(AsyncPolicy<T1, T2> first, AsyncPolicy<T1, T2> second) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Execute(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token) {
    return second.Execute(input, AsyncExecutable.Create((T1 i, CancellationToken t) => first.Execute(i, executable, t)), token);
  }

}