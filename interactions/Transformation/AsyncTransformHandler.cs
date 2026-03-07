using Interactions.Core;

namespace Interactions.Transformation;

internal sealed class AsyncTransformHandler<T1, T2, T3, T4>(
  Transformer<T1, T2> inputTransformer,
  AsyncHandler<T2, T3> inner,
  Transformer<T3, T4> outputTransformer) : AsyncHandler<T1, T4> {

  protected override async ValueTask<T4> ExecuteCore(T1 input, CancellationToken token = default) {
    return outputTransformer.Transform(await inner.Execute(inputTransformer.Transform(input), token));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}