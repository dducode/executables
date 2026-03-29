using Executables.Commands;
using JetBrains.Annotations;

namespace Executables.Tests.Commands;

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
    addCommand.Handle(Executable.Create((T obj) => list.Add(obj)).AsHandler());

    addCommand.Execute(item);
    Assert.Contains(item, list);
  }

  [Fact]
  public void ExecuteWithoutHandler() {
    var command = new Command<Unit>();
    Assert.False(command.Execute());
    IDisposable handle = command.Handle(Executable.Identity().AsHandler());
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
    using (command.Handle(Executable.Identity().AsHandler()))
      Assert.Throws<InvalidOperationException>(() => command.Handle(Executable.Identity().AsHandler()));
    command.Handle(Executable.Identity().AsHandler());
  }

}