using Interactions.Core.Commands;
using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Commands;

[TestSubject(typeof(AsyncCommand<>))]
public class AsyncCommandTest {

  [Theory]
  [InlineData(10)]
  [InlineData(1e-10f)]
  [InlineData(true)]
  [InlineData("ABC")]
  public async Task AddItemToList<T>(T item) {
    var list = new List<T>();
    var addCommand = new AsyncCommand<T>();
    addCommand.Handle(Handler.FromMethod((T obj) => list.Add(obj)).ToAsyncHandler());

    await addCommand.Execute(item);
    Assert.Contains(item, list);
  }

  [Fact]
  public async Task ExecuteWithoutHandler() {
    var command = new AsyncCommand<Unit>();
    Assert.False(await command.Execute());
    IDisposable handle = command.Handle(Handler.Identity().ToAsyncHandler());
    Assert.True(await command.Execute());
    handle.Dispose();
    Assert.False(await command.Execute());
  }

  [Fact]
  public async Task Cancel() {
    var command = new AsyncCommand<Unit>();
    command.Handle(Handler.Identity().ToAsyncHandler());
    var cts = new CancellationTokenSource();

    Assert.True(await command.Execute(cts.Token));
    await cts.CancelAsync();
    Assert.False(await command.Execute(cts.Token));
  }

  [Fact]
  public void PassNullHandler() {
    var command = new AsyncCommand<Unit>();
    Assert.Throws<ArgumentNullException>(() => command.Handle(null));
  }

  [Fact]
  public void AddHandlerWhenOtherExists() {
    var command = new AsyncCommand<Unit>();
    using (command.Handle(Handler.Identity().ToAsyncHandler()))
      Assert.Throws<InvalidOperationException>(() => command.Handle(Handler.Identity().ToAsyncHandler()));
    command.Handle(Handler.Identity().ToAsyncHandler());
  }

}