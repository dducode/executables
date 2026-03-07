using Interactions.Core.Handlers;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

internal sealed class HandlerProxy<T>(Observer<T> inner) : Handler<T, Unit> {

  protected override Unit ExecuteCore(T input) {
    inner.OnNext(input);
    return default;
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}