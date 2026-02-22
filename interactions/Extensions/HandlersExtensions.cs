using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Handlers;
using Interactions.Transformation;

namespace Interactions.Extensions;

public static class HandlersExtensions {

  [Pure]
  public static Handler<T1, T2> Catch<TException, T1, T2>(this Handler<T1, T2> handler, Func<TException, T1, T2> @catch) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(@catch, nameof(@catch));
    return new CatchHandler<TException, T1, T2>(handler, @catch);
  }

  [Pure]
  public static Handler<T1, T2> Catch<TException, T1, T2>(this Handler<T1, T2> handler, Action<TException, T1> @catch) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(@catch, nameof(@catch));
    return new CatchHandler<TException, T1, T2>(handler, (exception, arg2) => {
      @catch(exception, arg2);
      return default;
    });
  }

  [Pure]
  public static Handler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, Handler<T2, T3> nextHandler) {
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new CompositeHandler<T1, T2, T3>(handler, nextHandler);
  }

  [Pure]
  public static AsyncHandler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, AsyncHandler<T2, T3> nextHandler) {
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new AsyncCompositeHandler<T1, T2, T3>(handler.ToAsyncHandler(), nextHandler);
  }

  [Pure]
  public static Handler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, Func<T2, T3> nextHandler) {
    return handler.Next(Handler.FromMethod(nextHandler));
  }

  [Pure]
  public static AsyncHandler<T1, T3> Next<T1, T2, T3>(this Handler<T1, T2> handler, AsyncFunc<T2, T3> nextHandler) {
    return handler.Next(Handler.FromMethod(nextHandler));
  }

  [Pure]
  public static Handler<K1, K2> Transform<T1, T2, K1, K2>(this Handler<T1, T2> handler, Transformer<K1, T1> incoming, Transformer<T2, K2> outgoing) {
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new TransformHandler<K1, T1, T2, K2>(incoming, handler, outgoing);
  }

  [Pure]
  public static Handler<K1, K2> Transform<T1, T2, K1, K2>(this Handler<T1, T2> handler, Func<K1, T1> incoming, Func<T2, K2> outgoing) {
    return handler.Transform(Transformer.FromMethod(incoming), Transformer.FromMethod(outgoing));
  }

  [Pure]
  public static Handler<K1, T2> InputTransform<T1, T2, K1>(this Handler<T1, T2> handler, Transformer<K1, T1> incoming) {
    return handler.Transform(incoming, Transformer.Identity<T2>());
  }

  [Pure]
  public static Handler<T1, K2> OutputTransform<T1, T2, K2>(this Handler<T1, T2> handler, Transformer<T2, K2> outgoing) {
    return handler.Transform(Transformer.Identity<T1>(), outgoing);
  }

  [Pure]
  public static Handler<K1, T2> InputTransform<T1, T2, K1>(this Handler<T1, T2> handler, Func<K1, T1> incoming) {
    return handler.InputTransform(Transformer.FromMethod(incoming));
  }

  [Pure]
  public static Handler<T1, K2> OutputTransform<T1, T2, K2>(this Handler<T1, T2> handler, Func<T2, K2> outgoing) {
    return handler.OutputTransform(Transformer.FromMethod(outgoing));
  }

  [Pure]
  public static Handler<T1, T2> Do<T1, T2>(this Handler<T1, T2> handler, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new TransitiveHandler<T2>(action));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Do<T1, T2>(this Handler<T1, T2> handler, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return handler.Next(new AsyncTransitiveHandler<T2>(action));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Delay<T1, T2>(this Handler<T1, T2> handler, Func<T2, TimeSpan> timeDelay) {
    ExceptionsHelper.ThrowIfNull(timeDelay, nameof(timeDelay));
    return handler.Next(new DelayHandler<T2>(timeDelay));
  }

  [Pure]
  public static AsyncHandler<T1, T2> Delay<T1, T2>(this Handler<T1, T2> handler, TimeSpan timeDelay) {
    ExceptionsHelper.ThrowIfNull(timeDelay, nameof(timeDelay));
    return handler.Delay(delegate {
      return timeDelay;
    });
  }

  [Pure]
  public static Handler<T1, T2> Metrics<T1, T2>(this Handler<T1, T2> handler, IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new MetricsHandler<T1, T2>(handler, metrics, tag);
  }

}