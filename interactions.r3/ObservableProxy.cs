using Interactions.Core;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

internal sealed class ObservableProxy<T>(Handleable<T, Unit> inner) : Observable<T> {

  protected override IDisposable SubscribeCore(Observer<T> observer) {
    return inner.Handle(observer.AsHandler());
  }

}