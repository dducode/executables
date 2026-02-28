using Interactions.Core;

namespace Interactions.Transformation;

internal sealed class TransformHandler<T1, T2, T3, T4>(
  Transformer<T1, T2> inputTransformer,
  Handler<T2, T3> inner,
  Transformer<T3, T4> outputTransformer) : Handler<T1, T4> {

  public override T4 Handle(T1 input) {
    ThrowIfDisposed(nameof(TransformHandler<T1, T2, T3, T4>));
    return outputTransformer.Transform(inner.Handle(inputTransformer.Transform(input)));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}

internal sealed class AsyncTransformHandler<T1, T2, T3, T4>(
  Transformer<T1, T2> inputTransformer,
  AsyncHandler<T2, T3> inner,
  Transformer<T3, T4> outputTransformer) : AsyncHandler<T1, T4> {

  public override async ValueTask<T4> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncTransformHandler<T1, T2, T3, T4>));
    return outputTransformer.Transform(await inner.Handle(inputTransformer.Transform(input), token));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}