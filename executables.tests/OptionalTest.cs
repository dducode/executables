using JetBrains.Annotations;

namespace Executables.Tests;

[TestSubject(typeof(Optional<>))]
public class OptionalTest {

  [Fact]
  public void OptionalEquality() {
    Assert.Equal(new Optional<int>(2), new Optional<int>(2));
    Assert.Equal(Optional<int>.None, Optional<int>.None);
    Assert.True(Optional<string>.None == Optional<int>.None);

    Assert.NotEqual(new Optional<int>(2), new Optional<int>(3));
    Assert.NotEqual(new Optional<string>(null), Optional<string>.None);
    Assert.True(new Optional<int>(1) != Optional<string>.None);
  }

  [Fact]
  public async Task SuppressCancellationException() {
    var cts = new CancellationTokenSource();
    IAsyncQuery<int, Optional<int>> query = AsyncExecutable
      .Create(async (int x, CancellationToken token) => {
        await Task.Delay(10, token);
        return x * 2;
      })
      .SuppressException().OfType<OperationCanceledException>()
      .AsQuery();

    Optional<int> optional = await query.Send(5, token: cts.Token);

    Assert.True(optional.HasValue);
    Assert.Equal(10, optional.Value);
    Assert.Equal(10, optional.ValueOrDefault);

    await cts.CancelAsync();
    optional = await query.Send(2, token: cts.Token);

    Assert.False(optional.HasValue);
    Assert.Throws<InvalidOperationException>(() => optional.Value);
    Assert.Equal(0, optional.ValueOrDefault);
  }

  [Fact]
  public async Task SuppressManyExceptions() {
    var cts = new CancellationTokenSource();

    IAsyncQuery<int, Optional<int>> query = AsyncExecutable
      .Create(async ValueTask<int> (int _, CancellationToken token) => {
        await Task.Delay(10, token);
        throw new InvalidOperationException();
      })
      .SuppressException().OfType<OperationCanceledException>()
      .SuppressException().OfType<InvalidOperationException>()
      .AsQuery();

    Optional<int> optional = await query.Send(10, cts.Token);
    Assert.False(optional.HasValue);
    Assert.Equal(0, optional.ValueOrDefault);

    await cts.CancelAsync();

    optional = await query.Send(10, cts.Token);
    Assert.False(optional.HasValue);
    Assert.Equal(0, optional.ValueOrDefault);
  }

}