using Interactions.Core.Providers;
using Interactions.Core.Resolvers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(Handler))]
public class HandlersFactoryTest {

  [Fact]
  public void ThrowArgumentNullException() {
    Assert.Throws<ArgumentNullException>(() => Handler.Lazy((IResolver<Handler<Unit, Unit>>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.Lazy((IResolver<AsyncHandler<Unit, Unit>>)null));

    Assert.Throws<ArgumentNullException>(() => Handler.Dynamic((IProvider<Handler<Unit, Unit>>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.Dynamic((IProvider<AsyncHandler<Unit, Unit>>)null));

    Assert.Throws<ArgumentNullException>(() => Handler.Create(null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create<Unit, Unit>(null));

    Assert.Throws<ArgumentNullException>(() => Handler.Create(null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Handler.Create<Unit, Unit>(null));
  }

}