using Executables.Pipelines;

namespace Executables.Core.Pipelines;

internal sealed class AsyncAnonymousMiddleware<T1, T2, T3, T4>(AsyncFunc<T1, IAsyncExecutor<T2, T3>, T4> pipeline) : AsyncMiddleware<T1, T2, T3, T4> {

  public override ValueTask<T4> Invoke(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default) {
    return pipeline(input, executor, token);
  }

}