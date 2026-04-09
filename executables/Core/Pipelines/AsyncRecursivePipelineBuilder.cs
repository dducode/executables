using Executables.Internal;
using Executables.Pipelines;

namespace Executables.Core.Pipelines;

internal sealed class AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(
  AsyncPipelineBuilder<T1, T2, T3, T4> first,
  AsyncPipelineBuilder<T2, T5, T6, T3> second) : AsyncPipelineBuilder<T1, T5, T6, T4>(null) {

  public override IAsyncExecutor<T1, T4> End(IAsyncExecutor<T5, T6> executor) {
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return first.End(second.End(executor));
  }

}