using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(IdentityHandler<>))]
public class IdentityHandlerTest {

  [Theory]
  [InlineData(10, 10)]
  [InlineData(1e-10f, 1e-10f)]
  [InlineData(true, true)]
  public void ReturnIdentityValue<T>(T expected, T actual) {
    Handler<T, T> handler = Handler.Identity<T>();
    Assert.Equal(expected, handler.Execute(actual));
  }

}