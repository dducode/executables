using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AutoDisposedHandler<,>))]
public class AutoDisposedHandlerTest {

  [Fact]
  public void DisposeOnUnhandledException() {
    var query = new Query<string, int>();

    Handler<string, int> inner = TestHandler.IntParseHandler();
    query.Handle(inner.DisposeOnUnhandledException());

    Assert.Throws<ArgumentNullException>(() => query.Send(null));
    Assert.Throws<MissingHandlerException>(() => query.Send(null));
  }

}