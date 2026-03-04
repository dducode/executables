using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Extensions;

[TestSubject(typeof(HandlersExtensions))]
public class HandlersExtensionsTest {

  [Fact]
  public void ThrowExceptionFromNullHandler() {
    Assert.Throws<NullReferenceException>(() => ((Handler<Unit, Unit>)null).ToAsyncHandler());
  }

}