using Executables.Core.Handlers;
using Executables.Handling;
using Executables.Tests.Utils;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

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