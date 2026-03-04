using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Extensions;

[TestSubject(typeof(QueriesExtensions))]
public class QueriesExtensionsTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public void TrySendQuery(string expected, int value) {
    var query = new Query<int, string>();
    query.Handle(TestHandler.ToStringHandler<int>());

    Result<string> result = query.TrySend(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public void TrySendQueryWithoutHandler() {
    var query = new Query<Unit, Unit>();

    Result<Unit> result = query.TrySend();
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
  }

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public async Task TrySendAsyncQuery(string expected, int value) {
    var query = new AsyncQuery<int, string>();
    query.Handle(TestHandler.ToStringHandler<int>().ToAsyncHandler());

    Result<string> result = await query.TrySend(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task TrySendAsyncQueryWithoutHandler() {
    var query = new AsyncQuery<Unit, Unit>();

    Result<Unit> result = await query.TrySend();
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
  }

  [Fact]
  public async Task TrySendAsyncQueryWithCanceledToken() {
    var query = new AsyncQuery<Unit, Unit>();
    var cts = new CancellationTokenSource();
    query.Handle(Handler.Identity().ToAsyncHandler());

    await cts.CancelAsync();
    Result<Unit> result = await query.TrySend(cts.Token);
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is OperationCanceledException);
  }

  [Fact]
  public void ThrowExceptionFromNullQuery() {
    Assert.Throws<NullReferenceException>(() => ((Query<Unit, Unit>)null).ToAsyncQuery());
  }

}