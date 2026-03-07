using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class CompositePolicy<T1, T2>(Policy<T1, T2> first, Policy<T1, T2> second) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return second.Invoke(input, first.AsExecutable(executable));
  }

}

internal sealed class AsyncCompositePolicy<T1, T2>(AsyncPolicy<T1, T2> first, AsyncPolicy<T1, T2> second) : AsyncPolicy<T1, T2> {

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    return second.Invoke(input, first.AsExecutable(executable), token);
  }

}