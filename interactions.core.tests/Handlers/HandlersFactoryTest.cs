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

    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Action)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Func<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Func<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Action)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Func<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Handler.FromMethod((Func<Unit, Unit>)null));
  }

}