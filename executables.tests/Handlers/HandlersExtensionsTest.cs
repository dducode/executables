using Executables.Handling;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

[TestSubject(typeof(HandlersExtensions))]
public class HandlersExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullHandler() {
    Assert.Throws<NullReferenceException>(() => ((Handler<Unit, Unit>)null).ToAsyncHandler());
  }

}