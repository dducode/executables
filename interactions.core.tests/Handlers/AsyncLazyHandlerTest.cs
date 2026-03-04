using Interactions.Core.Extensions;
using Interactions.Core.Handlers;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncLazyHandler<,>))]
public class AsyncLazyHandlerTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("1E-10", 1e-10f)]
  [InlineData("True", true)]
  public async Task SimpleHandle<T>(string expected, T value) {
    AsyncHandler<T, string> handler = Handler.Lazy(Resolver.FromMethod(() => TestHandler.ToStringHandler<T>().ToAsyncHandler()));
    Assert.Equal(expected, await handler.Handle(value));
  }

  [Fact]
  public async Task ProvideNullHandler() {
    AsyncHandler<Unit, Unit> handler = Handler.Lazy(Resolver.FromMethod(AsyncHandler<Unit, Unit> () => null));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(default));
  }

  [Fact]
  public async Task DisposeInnerHandler() {
    AsyncHandler<Unit, Unit> inner = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = Handler.Lazy(Resolver.FromMethod(() => inner));
    await handler.Handle(default);
    handler.Dispose();
    Assert.True(inner.Disposed);
  }

}