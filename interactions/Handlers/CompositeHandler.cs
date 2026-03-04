using Interactions.Core;
using Interactions.Core.Extensions;

namespace Interactions.Handlers;

internal sealed class CompositeHandler<T1, T2, T3>(Handler<T1, T2> first, Handler<T2, T3> second) : Handler<T1, T3> {

  protected override T3 HandleCore(T1 input) {
    return second.Handle(first.Handle(input));
  }

  protected override void DisposeCore() {
    first.Compose(second).Dispose();
  }

}