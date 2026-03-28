using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AutoDetachableHandler<,,>))]
public class AutoDetachableHandlerTest {

  [Fact]
  public void DisposeOnException() {
    var query = new Query<string, int>();

    Handler<string, int> inner = TestHandler.IntParseHandler();
    query.Handle(inner.DisposeExternalHandle().OnException<ArgumentNullException>());

    Assert.Throws<ArgumentNullException>(() => query.Send(null));
    Assert.Throws<MissingHandlerException>(() => query.Send(null));
  }

}