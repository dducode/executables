using Interactions.Core;
using Interactions.Core.Extensions;

namespace Interactions.Handlers;

internal sealed class CompositeHandler<T1, T2, T3>(Handler<T1, T2> first, Handler<T2, T3> second) : Handler<T1, T3> {

  public override T3 Handle(T1 input) {
    ThrowIfDisposed(nameof(CompositeHandler<T1, T2, T3>));
    return second.Handle(first.Handle(input));
  }

  protected override void DisposeCore() {
    first.Compose(second).Dispose();
  }

}