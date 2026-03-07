using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Pipelines;

internal sealed class AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(
  AsyncPipelineBuilder<T1, T2, T3, T4> first,
  AsyncPipelineBuilder<T2, T5, T6, T3> second) : AsyncPipelineBuilder<T1, T5, T6, T4>(null) {

  public override IAsyncExecutable<T1, T4> End(IAsyncExecutable<T5, T6> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return first.End(second.End(executable));
  }

}