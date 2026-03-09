using JetBrains.Annotations;

namespace Interactions.Core.Tests;

[TestSubject(typeof(Handler))]
public class HandlerCreationTest {

  [Fact]
  public void ThrowArgumentNullException() {
    Assert.Throws<ArgumentNullException>(() => Handler.Lazy<Unit, Unit>(null));
    Assert.Throws<ArgumentNullException>(() => AsyncHandler.Lazy<Unit, Unit>(null));

    Assert.Throws<ArgumentNullException>(() => Handler.Dynamic<Unit, Unit>(null));
    Assert.Throws<ArgumentNullException>(() => AsyncHandler.Dynamic<Unit, Unit>(null));
  }

}