using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(RaceSuccessExecutable<,>))]
public class RaceSuccessExecutableTest {

  [Theory]
  [InlineData(10, 100, 2, 1)]
  [InlineData(100, 10, 11, 1)]
  public async Task SimpleRace(int firstDelay, int secondDelay, int expected, int value) {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        async (x, token) => {
          await Task.Delay(firstDelay, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(secondDelay, token);
          return x + 10;
        })
      .AsQuery();

    Assert.Equal(expected, await query.Send(value));
  }

  [Theory]
  [InlineData(10, 1)]
  [InlineData(50, 5)]
  public async Task ManyRace(int expected, int value) {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        },
        (x, _) => ValueTask.FromResult(x * 10),
        async (x, token) => {
          await Task.Delay(250, token);
          return x + 100;
        },
        async (x, token) => {
          await Task.Delay(100, token);
          return x * 100;
        })
      .AsQuery();

    Assert.Equal(expected, await query.Send(value));
  }

  [Fact]
  public async Task ReturnSuccessResultWhenAnyFaulted() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        (_, _) => throw new InvalidOperationException(),
        async (_, _) => throw new InvalidOperationException(),
        async (_, token) => {
          await Task.Delay(10, token);
          throw new InvalidOperationException();
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 2;
        },
        async (x, token) => {
          await Task.Delay(100, token);
          return x * 2;
        })
      .AsQuery();

    Assert.Equal(3, await query.Send(1));
  }

  [Fact]
  public async Task ThrowAggregateExceptionWhenAllFaulted() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        ValueTask<int> (_, _) => throw new InvalidOperationException(),
        async ValueTask<int> (_, _) => throw new OperationCanceledException()
      )
      .AsQuery();

    await Assert.ThrowsAsync<AggregateException>(async () => await query.Send(0));
  }

  [Fact]
  public async Task ThrowAggregateExceptionWhenAllFaultedAsync() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        async ValueTask<int> (_, token) => {
          await Task.Delay(10, token);
          throw new InvalidOperationException();
        },
        async ValueTask<int> (_, token) => {
          await Task.Delay(50, token);
          throw new OperationCanceledException();
        })
      .AsQuery();

    await Assert.ThrowsAsync<AggregateException>(async () => await query.Send(0));
  }

  [Fact]
  public async Task ThrowOperationCanceledExceptionWhenAllCanceled() {
    var cts = new CancellationTokenSource();

    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        ValueTask<int> (_, _) => throw new OperationCanceledException(),
        async ValueTask<int> (_, _) => throw new OperationCanceledException(),
        ValueTask<int> (_, token) => {
          cts.Cancel();
          return ValueTask.FromCanceled<int>(token);
        })
      .AsQuery();

    var ex = await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(0, cts.Token));
    Assert.Equal(cts.Token, ex.CancellationToken);
  }

  [Fact]
  public async Task ThrowOperationCanceledExceptionWhenAllCanceledAsync() {
    var cts = new CancellationTokenSource();
    var canceledCount = 0;

    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .RaceSuccess(
        async (x, token) => {
          try {
            await Task.Delay(50, token);
            return x * 2;
          }
          catch (OperationCanceledException) {
            Interlocked.Increment(ref canceledCount);
            throw;
          }
        },
        async (_, token) => {
          await cts.CancelAsync();
          try {
            await Task.Delay(10, token);
            return 0;
          }
          catch (OperationCanceledException) {
            Interlocked.Increment(ref canceledCount);
            throw;
          }
        },
        async (x, token) => {
          try {
            await Task.Delay(100, token);
            return x * 2;
          }
          catch (OperationCanceledException) {
            Interlocked.Increment(ref canceledCount);
            throw;
          }
        })
      .AsQuery();

    var ex = await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(0, cts.Token));
    Assert.Equal(3, canceledCount);
    Assert.Equal(cts.Token, ex.CancellationToken);
  }

}