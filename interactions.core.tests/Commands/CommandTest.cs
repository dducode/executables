using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Commands;

[TestSubject(typeof(Command<>))]
public class CommandTest {

  [Theory]
  [InlineData(10)]
  [InlineData(1e-10f)]
  [InlineData(true)]
  [InlineData("ABC")]
  public void AddItemToList<T>(T item) {
    var list = new List<T>();
    var addCommand = new Command<T>();
    addCommand.Handle(Handler.Create((T obj) => list.Add(obj)));

    addCommand.Execute(item);
    Assert.Contains(item, list);
  }

  [Fact]
  public void ExecuteWithoutHandler() {
    var command = new Command<Unit>();
    Assert.False(command.Execute());
    IDisposable handle = command.Handle(Handler.Identity());
    Assert.True(command.Execute());
    handle.Dispose();
    Assert.False(command.Execute());
  }

  [Fact]
  public void PassNullHandler() {
    var command = new Command<Unit>();
    Assert.Throws<ArgumentNullException>(() => command.Handle(null));
  }

  [Fact]
  public void AddHandlerWhenOtherExists() {
    var command = new Command<Unit>();
    using (command.Handle(Handler.Identity()))
      Assert.Throws<InvalidOperationException>(() => command.Handle(Handler.Identity()));
    command.Handle(Handler.Identity());
  }

}