using Interactions.Handling;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(HandlersExtensions))]
public class HandlersExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullHandler() {
    Assert.Throws<NullReferenceException>(() => ((Handler<Unit, Unit>)null).ToAsyncHandler());
  }

}