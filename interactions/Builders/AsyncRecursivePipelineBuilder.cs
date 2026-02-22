using Interactions.Core;

namespace Interactions.Builders;

internal sealed class AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(
  AsyncPipelineBuilder<T1, T2, T3, T4> first,
  AsyncPipelineBuilder<T2, T5, T6, T3> second) : AsyncPipelineBuilder<T1, T5, T6, T4>(null) {

  public override AsyncHandler<T1, T4> End(AsyncHandler<T5, T6> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return first.End(second.End(handler));
  }

}