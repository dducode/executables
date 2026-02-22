using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class PipelineHandler<T1, T2, T3, T4>(Pipeline<T1, T2, T3, T4> pipeline, Handler<T2, T3> next) : Handler<T1, T4> {

  public override T4 Handle(T1 input) {
    return pipeline.Invoke(input, next);
  }

}