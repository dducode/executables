namespace Interactions.Core;

internal sealed class AnonymousHandleable<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) : Handleable<T1, T2> {

  public override IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}

internal sealed class AsyncAnonymousHandleable<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) : AsyncHandleable<T1, T2> {

  public override IDisposable Handle(AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}