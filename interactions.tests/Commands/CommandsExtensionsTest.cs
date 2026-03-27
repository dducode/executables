using JetBrains.Annotations;
using CommandsExtensions = Interactions.Commands.CommandsExtensions;

namespace Interactions.Tests.Commands;

[TestSubject(typeof(CommandsExtensions))]
public class CommandsExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullCommand() {
    Assert.Throws<NullReferenceException>(() => Interactions.Commands.CommandsExtensions.ToAsyncCommand(((Command<Unit>)null)));
  }

}