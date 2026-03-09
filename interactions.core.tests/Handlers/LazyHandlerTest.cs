using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using Interactions.Core.Resolvers;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(LazyHandler<,>))]
public class LazyHandlerTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("1E-10", 1e-10f)]
  [InlineData("True", true)]
  public void SimpleHandle<T>(string expected, T value) {
    Handler<T, string> handler = Handler.Lazy(Resolver.Create(TestHandler.ToStringHandler<T>));
    Assert.Equal(expected, handler.Execute(value));
  }

  [Fact]
  public void ProvideNullHandler() {
    Handler<int, string> handler = Handler.Lazy(Resolver.Create(Handler<int, string> () => null));
    Assert.Throws<InvalidOperationException>(() => handler.Execute(10));
  }

  [Fact]
  public void DisposeInnerHandler() {
    Handler<Unit, Unit> inner = Executable.Identity().AsHandler();
    Handler<Unit, Unit> handler = Handler.Lazy(Resolver.Create(() => inner));
    handler.Execute(default);
    handler.Dispose();
    Assert.Throws<HandlerDisposedException>(() => inner.Execute(default));
  }

}