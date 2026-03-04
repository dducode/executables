using JetBrains.Annotations;

namespace Interactions.Core.Tests;

[TestSubject(typeof(Provider))]
public class ProviderTest {

  [Fact]
  public void PassNullProvider() {
    Assert.Throws<ArgumentNullException>(() => Provider.FromMethod<Unit>(null));
  }

}