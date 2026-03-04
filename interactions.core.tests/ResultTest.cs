using JetBrains.Annotations;

namespace Interactions.Core.Tests;

[TestSubject(typeof(Result<>))]
public class ResultTest {

  [Fact]
  public void FromResult() {
    var result = Result<int>.FromResult(10);
    Assert.True(result.IsValid);
    Assert.False(result.IsFailure);
    Assert.True(result.IsSuccess);
    Assert.True(result.TryGetValue(out _));

    Assert.Equal(10, result.Value);
    Assert.Null(result.Exception);
  }

  [Fact]
  public void FromException() {
    Result<Unit> result = Result<Unit>.FromException(new Exception());
    Assert.True(result.IsValid);
    Assert.True(result.IsFailure);
    Assert.False(result.IsSuccess);
    Assert.False(result.TryGetValue(out _));

    Assert.NotNull(result.Exception);
    Assert.Throws<Exception>(() => result.Value);
  }

  [Fact]
  public void InvalidResult() {
    var result = (Result<Unit>)default;
    Assert.False(result.IsValid);

    Assert.Throws<InvalidOperationException>(() => result.IsFailure);
    Assert.Throws<InvalidOperationException>(() => result.IsSuccess);
    Assert.Throws<InvalidOperationException>(() => result.Value);
    Assert.Throws<InvalidOperationException>(() => result.Exception);
    Assert.Throws<InvalidOperationException>(() => result.TryGetValue(out _));
  }

}