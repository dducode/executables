using Interactions.Core;

namespace Interactions.Transformation;

internal sealed class TransformHandler<T1, T2, T3, T4>(
  Transformer<T1, T2> inputTransformer,
  Handler<T2, T3> inner,
  Transformer<T3, T4> outputTransformer) : Handler<T1, T4> {

  protected override T4 ExecuteCore(T1 input) {
    return outputTransformer.Transform(inner.Execute(inputTransformer.Transform(input)));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}