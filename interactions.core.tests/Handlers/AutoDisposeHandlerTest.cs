using Interactions.Core.Handlers;
using Interactions.Core.Lifecycle;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AutoDisposeHandler<,,>))]
public class AutoDisposeHandlerTest {

  [Fact]
  public void DisposeOnException() {
    var handle = new DisposeHandle();
    var query = new Query<string, int>();

    Handler<string, int> inner = TestHandler.IntParseHandler();
    handle.Register(query.Handle(inner.DisposeOnException<ArgumentNullException>(handle)));

    Assert.Throws<ArgumentNullException>(() => query.Execute(null));
    Assert.Throws<MissingHandlerException>(() => query.Execute(null));
  }

}