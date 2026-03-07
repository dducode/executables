using Interactions.Core.Handlers;
using Interactions.Core.Queries;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Queries;

[TestSubject(typeof(Query<,>))]
public class QueryTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("1E-10", 1e-10f)]
  [InlineData("True", true)]
  public void GetStringRepresentation<T>(string expected, T value) {
    var query = new Query<T, string>();
    using IDisposable handle = query.Handle(TestHandler.ToStringHandler<T>());
    Assert.Equal(expected, query.Execute(value));
  }

  [Fact]
  public void SendWithoutHandler() {
    var query = new Query<Unit, Unit>();
    Assert.Throws<MissingHandlerException>(() => query.Execute(default));
    IDisposable handle = query.Handle(Handler.Identity());
    query.Execute(default);
    handle.Dispose();
    Assert.Throws<MissingHandlerException>(() => query.Execute(default));
  }

  [Fact]
  public void PassNullHandler() {
    var query = new Query<Unit, Unit>();
    Assert.Throws<ArgumentNullException>(() => query.Handle(null));
  }

  [Fact]
  public void AddHandlerWhenOtherExists() {
    var query = new Query<Unit, Unit>();
    using (query.Handle(Handler.Identity()))
      Assert.Throws<InvalidOperationException>(() => query.Handle(Handler.Identity()));
    query.Handle(Handler.Identity());
  }

}