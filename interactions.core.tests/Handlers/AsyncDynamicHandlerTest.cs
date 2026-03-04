using Interactions.Core.Extensions;
using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncDynamicHandler<,>))]
public class AsyncDynamicHandlerTest {

  [Fact]
  public async Task MultiplyHandle() {
    var multiplier = 0;
    AsyncHandler<int, int> handler = Handler.Dynamic(Provider.FromMethod(() => {
      multiplier++;
      return Handler.FromAsyncMethod((int num, CancellationToken _) => new ValueTask<int>(num * multiplier));
    }));

    Assert.Equal(10, await handler.Handle(10));
    Assert.Equal(20, await handler.Handle(10));
    Assert.Equal(30, await handler.Handle(10));
  }

  [Fact]
  public async Task ProvideNullHandler() {
    AsyncHandler<Unit, Unit> handler = Handler.Dynamic(Provider.FromMethod(AsyncHandler<Unit, Unit> () => null));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(default));
  }

  [Fact]
  public async Task InnerHandlerNotDispose() {
    AsyncHandler<Unit, Unit> inner = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = Handler.Dynamic(Provider.FromMethod(() => inner));
    await handler.Handle(default);
    Assert.False(inner.Disposed);
  }

}