using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Builders;

public class AsyncPipelineBuilder<T1, T2, T3, T4> {

  private readonly AsyncPipeline<T1, T2, T3, T4> _pipeline;

  internal AsyncPipelineBuilder(AsyncPipeline<T1, T2, T3, T4> pipeline) {
    _pipeline = pipeline;
  }

  public AsyncPipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(AsyncPipeline<T2, T5, T6, T3> pipeline) {
    return new AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(this, new AsyncPipelineBuilder<T2, T5, T6, T3>(pipeline));
  }

  [Pure]
  public virtual AsyncHandler<T1, T4> End(AsyncHandler<T2, T3> handler) {
    return new AsyncPipelineHandler<T1, T2, T3, T4>(_pipeline, handler);
  }

}