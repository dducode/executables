using Executables.Commands;
using Executables.Core.Commands;
using JetBrains.Annotations;

namespace Executables.Tests.Commands;

[TestSubject(typeof(CompositeCommand<>))]
public class CompositeCommandTest {

  [Fact]
  public void ComposeTwoCommands() {
    var firstExecuted = false;
    var secondExecuted = false;

    ICommand<Unit> first = Executable.Create(() => firstExecuted = true).AsCommand();
    ICommand<Unit> second = Executable.Create(() => secondExecuted = true).AsCommand();

    ICommand<Unit> composed = first.Prepend(second);
    Assert.True(composed.Execute());
    Assert.True(firstExecuted);
    Assert.True(secondExecuted);

    firstExecuted = secondExecuted = false;

    ICommand<Unit> chained = first.Append(second);
    Assert.True(chained.Execute());
    Assert.True(firstExecuted);
    Assert.True(secondExecuted);
  }

  [Fact]
  public void NotExecuteFirstWhenSecondIsFailed() {
    var firstExecuted = false;

    ICommand<Unit> first = Executable.Create(() => firstExecuted = true).AsCommand();
    ICommand<Unit> second = Executable.Create(() => false).AsCommand();

    ICommand<Unit> composed = first.Prepend(second);
    Assert.False(composed.Execute());
    Assert.False(firstExecuted);
  }

  [Fact]
  public void NotExecuteSecondWhenFirstIsFailed() {
    var secondExecuted = false;

    ICommand<Unit> first = Executable.Create(() => false).AsCommand();
    ICommand<Unit> second = Executable.Create(() => secondExecuted = true).AsCommand();

    ICommand<Unit> composed = first.Append(second);
    Assert.False(composed.Execute());
    Assert.False(secondExecuted);
  }

}