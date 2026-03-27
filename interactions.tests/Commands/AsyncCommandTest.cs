using Interactions.Commands;
using Interactions.Handling;
using JetBrains.Annotations;

namespace Interactions.Tests.Commands;

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
    addCommand.Handle(Executable.Create((T obj) => list.Add(obj)).AsHandler().ToAsyncHandler());

    await addCommand.Execute(item);
    Assert.Contains(item, list);
  }

  [Fact]
  public async Task ExecuteWithoutHandler() {
    var command = new AsyncCommand<Unit>();
    Assert.False(await command.Execute());
    IDisposable handle = command.Handle(AsyncExecutable.Identity().AsHandler());
    Assert.True(await command.Execute());
    handle.Dispose();
    Assert.False(await command.Execute());
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();

    var command = new AsyncCommand<Unit>();
    command.Handle(AsyncExecutable.Create(async (Unit _, CancellationToken token) => {
      await cts.CancelAsync();
      await Task.Delay(10, token);
    }).AsHandler());

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
    using (command.Handle(AsyncExecutable.Identity().AsHandler()))
      Assert.Throws<InvalidOperationException>(() => command.Handle(AsyncExecutable.Identity().AsHandler()));
    command.Handle(AsyncExecutable.Identity().AsHandler());
  }

}