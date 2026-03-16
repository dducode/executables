using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncHandler<,>))]
public class AsyncHandlerTest {

  [Fact]
  public void DisposeHandler() {
    AsyncHandler<Unit, Unit> handler = AsyncExecutable.Identity().AsHandler();
    handler.Dispose();
    Assert.True(handler.Disposed);
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    AsyncHandler<Unit, Unit> handler = AsyncExecutable.Identity().AsHandler();
    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await handler.Handle(default, cts.Token));
  }

}