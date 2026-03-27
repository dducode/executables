using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Lifecycle;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Core.Handlers;

[TestSubject(typeof(AutoDisposeHandler<,,>))]
public class AutoDisposeHandlerTest {

  [Fact]
  public void DisposeOnException() {
    var handle = new DisposeHandle();
    var query = new Query<string, int>();

    Handler<string, int> inner = TestHandler.IntParseHandler();
    handle.Register(query.Handle(inner.DisposeOnException<ArgumentNullException>(handle)));

    Assert.Throws<ArgumentNullException>(() => query.Send(null));
    Assert.Throws<MissingHandlerException>(() => query.Send(null));
  }

}