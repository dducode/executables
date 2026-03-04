using JetBrains.Annotations;

namespace Interactions.Core.Tests;

[TestSubject(typeof(Resolver))]
public class ResolverTest {

  [Fact]
  public void PassNullResolver() {
    Assert.Throws<ArgumentNullException>(() => Resolver.FromMethod<Unit>(null));
  }

}