using Interactions.Core;

namespace Interactions;

public class AsyncMap<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing)
  : AsyncExecutionOperator<T1, T2, T3, T4> {

  internal static AsyncMap<T1, T1, T2, T2> Identity { get; } = new(Executable.Identity<T1>(), Executable.Identity<T2>());

  public override async ValueTask<T4> Invoke(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default) {
    return outgoing.Execute(await next.Execute(incoming.Execute(input), token));
  }

}