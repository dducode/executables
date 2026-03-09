using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using Interactions.Core.Resolvers;
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
    AsyncHandler<T, string> handler = AsyncHandler.Lazy(Resolver.Create(() => TestHandler.ToStringHandler<T>().ToAsyncHandler()));
    Assert.Equal(expected, await handler.Execute(value));
  }

  [Fact]
  public async Task ProvideNullHandler() {
    AsyncHandler<Unit, Unit> handler = AsyncHandler.Lazy(Resolver.Create(AsyncHandler<Unit, Unit> () => null));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Execute(default));
  }

  [Fact]
  public async Task DisposeInnerHandler() {
    AsyncHandler<Unit, Unit> inner = Executable.Identity().AsHandler().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = AsyncHandler.Lazy(Resolver.Create(() => inner));
    await handler.Execute(default);
    handler.Dispose();
    Assert.True(inner.Disposed);
  }

}