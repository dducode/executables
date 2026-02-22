using Interactions.Core;

namespace Interactions.Builders;

internal sealed class RecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(
  PipelineBuilder<T1, T2, T5, T6> first,
  PipelineBuilder<T2, T3, T4, T5> second) : PipelineBuilder<T1, T3, T4, T6>(null) {

  public override Handler<T1, T6> End(Handler<T3, T4> handler) {
    return first.End(second.End(handler));
  }

}