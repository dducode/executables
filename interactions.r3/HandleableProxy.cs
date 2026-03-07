using Interactions.Core.Handleables;
using Interactions.Core.Handlers;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

internal sealed class HandleableProxy<T>(Observable<T> inner) : Handleable<T, Unit> {

  public override IDisposable Handle(Handler<T, Unit> handler) {
    return inner.Subscribe(handler.AsObserver());
  }

}