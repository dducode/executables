using Interactions.Core;
using Interactions.Core.Extensions;

namespace Interactions.Handlers;

internal sealed class AsyncCompositeHandler<T1, T2, T3>(AsyncHandler<T1, T2> first, AsyncHandler<T2, T3> second) : AsyncHandler<T1, T3> {

  protected override async ValueTask<T3> HandleCore(T1 input, CancellationToken token = default) {
    return await second.Handle(await first.Handle(input, token), token);
  }

  protected override void DisposeCore() {
    first.Compose(second).Dispose();
  }

}