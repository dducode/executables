using Interactions.Core;
using Interactions.Core.Handlers;

namespace Interactions.Transformation;

internal sealed class SymmetricTransformHandler<T1, T2>(SymmetricTransformer<T1, T2> transformer, Handler<T2, T2> inner) : Handler<T1, T1> {

  protected override T1 ExecuteCore(T1 input) {
    return transformer.InverseTransform(inner.Execute(transformer.Transform(input)));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}

internal sealed class AsyncSymmetricTransformHandler<T1, T2>(SymmetricTransformer<T1, T2> transformer, AsyncHandler<T2, T2> inner) : AsyncHandler<T1, T1> {

  protected override async ValueTask<T1> ExecuteCore(T1 input, CancellationToken token = default) {
    return transformer.InverseTransform(await inner.Execute(transformer.Transform(input), token));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}