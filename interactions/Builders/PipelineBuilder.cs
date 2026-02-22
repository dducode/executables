using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Builders;

public class PipelineBuilder<T1, T2, T3, T4> {

  private readonly Pipeline<T1, T2, T3, T4> _pipeline;

  internal PipelineBuilder(Pipeline<T1, T2, T3, T4> pipeline) {
    _pipeline = pipeline;
  }

  public PipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(Pipeline<T2, T5, T6, T3> pipeline) {
    return new RecursivePipelineBuilder<T1, T2, T5, T6, T3, T4>(this, new PipelineBuilder<T2, T5, T6, T3>(pipeline));
  }

  [Pure]
  public virtual Handler<T1, T4> End(Handler<T2, T3> handler) {
    return new PipelineHandler<T1, T2, T3, T4>(_pipeline, handler);
  }

}