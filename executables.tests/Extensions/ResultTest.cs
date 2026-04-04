using Executables.Handling;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Extensions;

[TestSubject(typeof(Result<>))]
public class ResultTest {

  [Fact]
  public void ResultEquality() {
    const string errorMessage = "Some error was occurred";

    Assert.Equal(Result<int>.FromResult(1), Result<int>.FromResult(1));
    Assert.NotEqual(Result<int>.FromResult(1), Result<int>.FromResult(2));
    Assert.NotEqual(
      Result<int>.FromException(new InvalidOperationException(errorMessage)),
      Result<int>.FromResult(2)
    );
    Assert.NotEqual(
      Result<int>.FromException(new InvalidOperationException(errorMessage)),
      Result<int>.FromException(new InvalidOperationException(errorMessage))
    );
  }

  [Fact]
  public void FromResult() {
    var result = Result<int>.FromResult(10);
    Assert.False(result.IsFailure);
    Assert.True(result.IsSuccess);
    Assert.True(result.TryGetValue(out _));

    Assert.Null(result.Exception);
    result.ThrowIfFailure();
    Assert.Equal(10, result.Value);
  }

  [Fact]
  public void FromException() {
    Result<Unit> result = Result<Unit>.FromException(new TestException());
    Assert.True(result.IsFailure);
    Assert.False(result.IsSuccess);
    Assert.False(result.TryGetValue(out _));

    Assert.NotNull(result.Exception);
    Assert.Throws<TestException>(() => result.ThrowIfFailure());
    var invalidOperationException = Assert.Throws<InvalidOperationException>(() => result.Value);
    Assert.Equal(invalidOperationException.InnerException, result.Exception);
  }

  [Fact]
  public void DefaultResultIsSuccess() {
    var result = (Result<object>)default;

    Assert.False(result.IsFailure);
    Assert.True(result.IsSuccess);
    Assert.Null(result.Value);
    Assert.Null(result.Exception);
    Assert.True(result.TryGetValue(out _));
  }

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public void TryExecuteQuery(string expected, int value) {
    var baseQuery = new Query<int, string>();
    IDisposable handle = baseQuery.Handle(Executable.Create((int x) => x.ToString()).AsHandler());
    IQuery<int, Result<string>> query = baseQuery.WithResult().AsQuery();

    Result<string> result = query.Send(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);

    handle.Dispose();

    result = query.Send(value);
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
    Assert.Throws<MissingHandlerException>(() => result.ThrowIfFailure());
  }

  [Theory]
  [InlineData("10", 10)]
  [InlineData("20", 20)]
  [InlineData("30", 30)]
  public async Task TryExecuteAsyncQuery(string expected, int value) {
    var baseQuery = new AsyncQuery<int, string>();
    IDisposable handle = baseQuery.Handle(AsyncExecutable.Create((int x, CancellationToken _) => ValueTask.FromResult(x.ToString())).AsHandler());
    IAsyncQuery<int, Result<string>> query = baseQuery.WithResult().AsQuery();

    Result<string> result = await query.Send(value);
    Assert.True(result.IsSuccess);
    Assert.Equal(expected, result.Value);

    handle.Dispose();

    result = await query.Send(value);
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is MissingHandlerException);
    Assert.Throws<MissingHandlerException>(() => result.ThrowIfFailure());
  }

  [Fact]
  public async Task TryExecuteAsyncQueryWithCanceledToken() {
    IAsyncQuery<Unit, Result<Unit>> query = AsyncExecutable
      .Create((Unit _, CancellationToken token) => {
        token.ThrowIfCancellationRequested();
        return default;
      })
      .WithResult()
      .AsQuery();

    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    Result<Unit> result = await query.Send(cts.Token);
    Assert.True(result.IsFailure);
    Assert.True(result.Exception is OperationCanceledException);
    Assert.Throws<OperationCanceledException>(() => result.ThrowIfFailure());
  }

}

file class TestException : Exception;