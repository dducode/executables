using Interactions.Core;

namespace Interactions.Transformation;

internal sealed class SymmetricTransformHandler<T1, T2>(SymmetricTransformer<T1, T2> transformer, Handler<T2, T2> inner) : Handler<T1, T1> {

  protected override T1 ExecuteCore(T1 input) {
    return transformer.InverseTransform(inner.Execute(transformer.Transform(input)));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}