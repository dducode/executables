using Interactions.Commands;
using Interactions.Core.Commands;
using JetBrains.Annotations;

namespace Interactions.Tests.Commands;

[TestSubject(typeof(CompositeCommand<>))]
public class CompositeCommandTest {

  [Fact]
  public void ComposeTwoCommands() {
    var firstExecuted = false;
    var secondExecuted = false;

    ICommand<Unit> first = Executable
      .Create(() => firstExecuted = true)
      .AsCommand();

    ICommand<Unit> second = Executable
      .Create(() => secondExecuted = true)
      .AsCommand();

    ICommand<Unit> composed = first.Compose(second);
    Assert.True(composed.Execute());
    Assert.True(firstExecuted);
    Assert.True(secondExecuted);
  }

  [Fact]
  public void NotExecuteSecondWhenFirstIsFailed() {
    var secondExecuted = false;

    ICommand<Unit> first = Executable
      .Create(() => false)
      .AsCommand();

    ICommand<Unit> second = Executable
      .Create(() => secondExecuted = true)
      .AsCommand();

    ICommand<Unit> composed = first.Compose(second);
    Assert.False(composed.Execute());
    Assert.False(secondExecuted);
  }

}