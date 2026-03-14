using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using Interactions.Core.Queries;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Queries;

[TestSubject(typeof(AsyncQuery<,>))]
public class AsyncQueryTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("1E-10", 1e-10f)]
  [InlineData("True", true)]
  private async Task GetStringRepresentation<T>(string expected, T value) {
    var query = new AsyncQuery<T, string>();
    query.Handle(TestHandler.ToStringHandler<T>().ToAsyncHandler());
    Assert.Equal(expected, await query.Send(value));
  }

  [Fact]
  private async Task Cancel() {
    var cts = new CancellationTokenSource();
    var query = new AsyncQuery<Unit, Unit>();
    query.Handle(Executable.Identity().AsHandler().ToAsyncHandler());
    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(default, cts.Token));
  }

  [Fact]
  public async Task SendWithoutHandler() {
    var query = new AsyncQuery<Unit, Unit>();
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Send());
    IDisposable handle = query.Handle(Executable.Identity().AsHandler().ToAsyncHandler());
    await query.Send();
    handle.Dispose();
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Send());
  }

  [Fact]
  public void PassNullHandler() {
    var query = new AsyncQuery<Unit, Unit>();
    Assert.Throws<ArgumentNullException>(() => query.Handle(null));
  }

  [Fact]
  public void AddHandlerWhenOtherExists() {
    var query = new AsyncQuery<Unit, Unit>();
    using (query.Handle(Executable.Identity().AsHandler().ToAsyncHandler()))
      Assert.Throws<InvalidOperationException>(() => query.Handle(Executable.Identity().AsHandler().ToAsyncHandler()));
    query.Handle(Executable.Identity().AsHandler().ToAsyncHandler());
  }

}