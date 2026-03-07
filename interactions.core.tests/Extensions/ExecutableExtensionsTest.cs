using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using Interactions.Core.Queries;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Extensions;

[TestSubject(typeof(ExecutableExtensions))]
public class ExecutableExtensionsTest {

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public void TryExecuteQuery(string expected, int value) {
    var query = new Query<int, string>();
    query.Handle(TestHandler.ToStringHandler<int>());

    Result<string> result = query.TryExecute(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public void TryExecuteQueryWithoutHandler() {
    var query = new Query<Unit, Unit>();

    Result<Unit> result = query.TryExecute();
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
  }

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public async Task TryExecuteAsyncQuery(string expected, int value) {
    var query = new AsyncQuery<int, string>();
    query.Handle(TestHandler.ToStringHandler<int>().ToAsyncHandler());

    Result<string> result = await query.TryExecute(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);
  }

  [Fact]
  public async Task TryExecuteAsyncQueryWithoutHandler() {
    var query = new AsyncQuery<Unit, Unit>();

    Result<Unit> result = await query.TryExecute();
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
  }

  [Fact]
  public async Task TryExecuteAsyncQueryWithCanceledToken() {
    var query = new AsyncQuery<Unit, Unit>();
    var cts = new CancellationTokenSource();
    query.Handle(Handler.Identity().ToAsyncHandler());

    await cts.CancelAsync();
    Result<Unit> result = await query.TryExecute(cts.Token);
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is OperationCanceledException);
  }

  [Fact]
  public void ThrowExceptionFromNullQuery() {
    Assert.Throws<NullReferenceException>(() => ((Query<Unit, Unit>)null).ToAsyncExecutable());
  }

}