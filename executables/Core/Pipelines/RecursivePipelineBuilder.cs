using Executables.Internal;
using Executables.Pipelines;

namespace Executables.Core.Pipelines;

internal sealed class RecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(
  PipelineBuilder<T1, T2, T5, T6> first,
  PipelineBuilder<T2, T3, T4, T5> second) : PipelineBuilder<T1, T3, T4, T6>(null) {

  public override IExecutor<T1, T6> End(IExecutor<T3, T4> executor) {
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return first.End(second.End(executor));
  }

}