using JetBrains.Annotations;
using CommandsExtensions = Executables.Commands.CommandsExtensions;

namespace Executables.Tests.Commands;

[TestSubject(typeof(CommandsExtensions))]
public class CommandsExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullCommand() {
    Assert.Throws<NullReferenceException>(() => CommandsExtensions.ToAsyncCommand(((Command<Unit>)null)));
  }

}