using Interactions.Core.Handlers;
using Interactions.Core.Providers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncDynamicHandler<,>))]
public class AsyncDynamicHandlerTest {

  [Fact]
  public async Task MultiplyHandle() {
    var multiplier = 0;
    AsyncHandler<int, int> handler = Handler.Dynamic(Provider.Create(() => {
      multiplier++;
      return AsyncHandler.Create((int num, CancellationToken _) => new ValueTask<int>(num * multiplier));
    }));

    Assert.Equal(10, await handler.Execute(10));
    Assert.Equal(20, await handler.Execute(10));
    Assert.Equal(30, await handler.Execute(10));
  }

  [Fact]
  public async Task ProvideNullHandler() {
    AsyncHandler<Unit, Unit> handler = Handler.Dynamic(Provider.Create(AsyncHandler<Unit, Unit> () => null));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Execute(default));
  }

  [Fact]
  public async Task InnerHandlerNotDispose() {
    AsyncHandler<Unit, Unit> inner = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = Handler.Dynamic(Provider.Create(() => inner));
    await handler.Execute(default);
    Assert.False(inner.Disposed);
  }

}