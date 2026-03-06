using Interactions.Core.Extensions;

namespace Interactions.Core.Handlers;

internal sealed class CompositeHandler<T1, T2, T3>(Handler<T1, T2> first, Handler<T2, T3> second) : Handler<T1, T3> {

  protected override T3 ExecuteCore(T1 input) {
    return second.Execute(first.Execute(input));
  }

  protected override void DisposeCore() {
    first.Compose(second).Dispose();
  }

}