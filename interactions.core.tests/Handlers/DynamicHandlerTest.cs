using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(DynamicHandler<,>))]
public class DynamicHandlerTest {

  [Fact]
  public void MultiplyHandle() {
    var multiplier = 0;
    Handler<int, int> handler = Handler.Dynamic(Provider.FromMethod(() => {
      multiplier++;
      return Handler.FromMethod<int, int>(num => num * multiplier);
    }));

    Assert.Equal(10, handler.Execute(10));
    Assert.Equal(20, handler.Execute(10));
    Assert.Equal(30, handler.Execute(10));
  }

  [Fact]
  public void ProvideNullHandler() {
    Handler<Unit, Unit> handler = Handler.Dynamic(Provider.FromMethod(Handler<Unit, Unit> () => null));
    Assert.Throws<InvalidOperationException>(() => handler.Execute(default));
  }

  [Fact]
  public void InnerHandlerNotDispose() {
    Handler<Unit, Unit> inner = Handler.Identity();
    Handler<Unit, Unit> handler = Handler.Dynamic(Provider.FromMethod(() => inner));
    handler.Execute(default);
    Assert.False(inner.Disposed);
  }

}