using Executables.Core.Handlers;
using Executables.Handling;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

[TestSubject(typeof(AsyncAnonymousDisposeHandler<,>))]
public class AsyncAnonymousDisposeHandlerTest {

  [Fact]
  public async Task InvokeInnerHandler() {
    AsyncHandler<Unit, bool> handler = AsyncExecutable.Create(_ => new ValueTask<bool>(true)).AsHandler().OnDispose(() => { });
    Assert.True(await handler.Handle(default));
  }

  [Fact]
  public void WrapperDisposesInner() {
    var disposed = false;
    AsyncHandler<Unit, Unit> inner = Executable.Identity().ToAsyncExecutable().AsHandler();
    AsyncHandler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    handler.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
    Assert.True(handler.Disposed);
  }

  [Fact]
  public void InnerDisposesWrapper() {
    var disposed = false;
    AsyncHandler<Unit, Unit> inner = Executable.Identity().ToAsyncExecutable().AsHandler();
    AsyncHandler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    inner.Dispose();

    Assert.True(disposed);
    Assert.True(handler.Disposed);
    Assert.True(inner.Disposed);
  }

}