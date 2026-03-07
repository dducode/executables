using System.Diagnostics.Contracts;
using Interactions.Core;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

public static class R3Extensions {

  [Pure]
  public static Observer<T> AsObserver<T>(this Handler<T, Unit> handler) {
    return new ObserverProxy<T>(handler);
  }

  [Pure]
  public static Handler<T, Unit> AsHandler<T>(this Observer<T> observer) {
    return new HandlerProxy<T>(observer);
  }

  [Pure]
  public static Observable<T> AsObservable<T>(this Handleable<T, Unit> handleable) {
    return new ObservableProxy<T>(handleable);
  }

  [Pure]
  public static Handleable<T, Unit> AsHandleable<T>(this Observable<T> observable) {
    return new HandleableProxy<T>(observable);
  }

}