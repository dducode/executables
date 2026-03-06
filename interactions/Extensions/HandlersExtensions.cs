using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Handlers;
using Interactions.Transformation;

namespace Interactions.Extensions;

public static partial class HandlersExtensions {

  [Pure]
  public static Handler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, Handler<T2, T3> nextHandler) {
    ExceptionsHelper.ThrowIfNullReference(handler);
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new CompositeHandler<T1, T2, T3>(handler, nextHandler);
  }

  [Pure]
  public static AsyncHandler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, AsyncHandler<T2, T3> nextHandler) {
    ExceptionsHelper.ThrowIfNullReference(handler);
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new AsyncCompositeHandler<T1, T2, T3>(handler.ToAsyncHandler(), nextHandler);
  }

  [Pure]
  public static Handler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, Func<T2, T3> nextHandler) {
    return handler.Next(Handler.FromMethod(nextHandler));
  }

  [Pure]
  public static AsyncHandler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, AsyncFunc<T2, T3> nextHandler) {
    return handler.Next(Handler.FromAsyncMethod(nextHandler));
  }

  [Pure]
  public static Handler<T1, T4> Transform<T1, T2, T3, T4>(this Handler<T2, T3> handler, Transformer<T1, T2> incoming, Transformer<T3, T4> outgoing) {
    ExceptionsHelper.ThrowIfNullReference(handler);
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new TransformHandler<T1, T2, T3, T4>(incoming, handler, outgoing);
  }

  [Pure]
  public static Handler<T1, T4> Transform<T1, T2, T3, T4>(this Handler<T2, T3> handler, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return handler.Transform(Transformer.FromMethod(incoming), Transformer.FromMethod(outgoing));
  }

  [Pure]
  public static Handler<T1, T3> InputTransform<T1, T2, T3>(this Handler<T2, T3> handler, Transformer<T1, T2> incoming) {
    return handler.Transform(incoming, Transformer.Identity<T3>());
  }

  [Pure]
  public static Handler<T1, T3> OutputTransform<T1, T2, T3>(this Handler<T1, T2> handler, Transformer<T2, T3> outgoing) {
    return handler.Transform(Transformer.Identity<T1>(), outgoing);
  }

  [Pure]
  public static Handler<T1, T3> InputTransform<T1, T2, T3>(this Handler<T2, T3> handler, Func<T1, T2> incoming) {
    return handler.InputTransform(Transformer.FromMethod(incoming));
  }

  [Pure]
  public static Handler<T1, T3> OutputTransform<T1, T2, T3>(this Handler<T1, T2> handler, Func<T2, T3> outgoing) {
    return handler.OutputTransform(Transformer.FromMethod(outgoing));
  }

  [Pure]
  [Obsolete("Use Tap() instead")]
  public static Handler<T1, T2> Do<T1, T2>(this Handler<T1, T2> handler, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new TransitiveHandler<T2>(action));
  }

  [Pure]
  [Obsolete("Use Tap() instead")]
  public static AsyncHandler<T1, T2> Do<T1, T2>(this Handler<T1, T2> handler, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new AsyncTransitiveHandler<T2>(action));
  }

  [Pure]
  public static Handler<T1, T2> Tap<T1, T2>(this Handler<T1, T2> handler, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new TransitiveHandler<T2>(action));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Tap<T1, T2>(this Handler<T1, T2> handler, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new AsyncTransitiveHandler<T2>(action));
  }

  [Pure]
  public static Handler<T1, T2> OnDispose<T1, T2>(this Handler<T1, T2> handler, Action dispose) {
    ExceptionsHelper.ThrowIfNullReference(handler);
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

}