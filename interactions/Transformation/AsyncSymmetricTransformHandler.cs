using Interactions.Core;

namespace Interactions.Transformation;

internal sealed class AsyncSymmetricTransformHandler<T1, T2>(SymmetricTransformer<T1, T2> transformer, AsyncHandler<T2, T2> inner) : AsyncHandler<T1, T1> {

  protected override async ValueTask<T1> ExecuteCore(T1 input, CancellationToken token = default) {
    return transformer.InverseTransform(await inner.Execute(transformer.Transform(input), token));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}