using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class AsyncPipelineHandler<T1, T2, T3, T4>(AsyncPipeline<T1, T2, T3, T4> pipeline, AsyncHandler<T2, T3> next) : AsyncHandler<T1, T4> {

  public override ValueTask<T4> Handle(T1 input, CancellationToken token = default) {
    return pipeline.Invoke(input, next, token);
  }

}