using Interactions.Core.Handlers;
using Interactions.Handling;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AsyncAnonymousDisposeHandler<,>))]
public class AsyncAnonymousDisposeHandlerTest {

  [Fact]
  public async Task InvokeInnerHandler() {
    AsyncHandler<Unit, bool> handler = AsyncExecutable.Create(_ => new ValueTask<bool>(true)).AsHandler().OnDispose(() => { });
    Assert.True(await handler.Handle(default));
  }

  [Fact]
  public void DisposeHandler() {
    var disposed = false;
    AsyncHandler<Unit, Unit> inner = AsyncExecutable.Identity().AsHandler();
    AsyncHandler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    handler.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
  }

  [Fact]
  public async Task AsyncDisposeHandler() {
    var disposed = false;
    AsyncHandler<Unit, Unit> inner = AsyncExecutable.Identity().AsHandler();
    AsyncHandler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    await handler.DisposeAsync();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
  }

}