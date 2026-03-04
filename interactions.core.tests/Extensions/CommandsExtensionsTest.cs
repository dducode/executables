using Interactions.Core.Commands;
using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Extensions;

[TestSubject(typeof(CommandsExtensions))]
public class CommandsExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullCommand() {
    Assert.Throws<NullReferenceException>(() => ((Command<Unit>)null).ToAsyncCommand());
  }

}