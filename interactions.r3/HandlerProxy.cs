using Interactions.Core;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

internal sealed class HandlerProxy<T>(Observer<T> inner) : Handler<T, Unit> {

  public override Unit Handle(T input) {
    ThrowIfDisposed(nameof(HandlerProxy<T>));
    inner.OnNext(input);
    return default;
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}