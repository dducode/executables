using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Handlers;
using Interactions.Transformation;

namespace Interactions.Handlers;

public static partial class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T4> Transform<T1, T2, T3, T4>(
    this AsyncHandler<T2, T3> handler,
    Transformer<T1, T2> incoming,
    Transformer<T3, T4> outgoing) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new AsyncTransformHandler<T1, T2, T3, T4>(incoming, handler, outgoing);
  }

  [Pure]
  public static AsyncHandler<T1, T4> Transform<T1, T2, T3, T4>(this AsyncHandler<T2, T3> handler, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return handler.Transform(Transformer.Create(incoming), Transformer.Create(outgoing));
  }

  [Pure]
  public static AsyncHandler<T1, T3> InputTransform<T1, T2, T3>(this AsyncHandler<T2, T3> handler, Transformer<T1, T2> incoming) {
    return handler.Transform(incoming, Transformer.Identity<T3>());
  }

  [Pure]
  public static AsyncHandler<T1, T3> OutputTransform<T1, T2, T3>(this AsyncHandler<T1, T2> handler, Transformer<T2, T3> outgoing) {
    return handler.Transform(Transformer.Identity<T1>(), outgoing);
  }

  [Pure]
  public static AsyncHandler<T1, T3> InputTransform<T1, T2, T3>(this AsyncHandler<T2, T3> handler, Func<T1, T2> incoming) {
    return handler.InputTransform(Transformer.Create(incoming));
  }

  [Pure]
  public static AsyncHandler<T1, T3> OutputTransform<T1, T2, T3>(this AsyncHandler<T1, T2> handler, Func<T2, T3> outgoing) {
    return handler.OutputTransform(Transformer.Create(outgoing));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Tap<T1, T2>(this AsyncHandler<T1, T2> handler, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Then(new AsyncTransitiveHandler<T2>(action));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Tap<T1, T2>(this AsyncHandler<T1, T2> handler, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Then(new TransitiveHandler<T2>(action));
  }

}