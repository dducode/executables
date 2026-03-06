using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncPreventReentrancePolicy<,>))]
public class AsyncPreventReentrancePolicyTest {

  [Fact]
  public async Task SequentialExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, Wait, CancellationToken.None);
    await policy.Execute(default, Wait, CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();
    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await policy.Execute(default, Wait, cts.Token));
  }

  [Fact]
  public async Task MultiplyExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    ValueTask<Unit> first = policy.Execute(default, Wait, CancellationToken.None);
    ValueTask<Unit> second = policy.Execute(default, Wait, CancellationToken.None);
    await Task.WhenAll(first.AsTask(), second.AsTask());
  }

  [Fact]
  public async Task NestedExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, async (input, token) => {
      await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(input, Wait, token));
      return default;
    }, CancellationToken.None);
  }

  [Fact]
  public async Task ParallelSequentialExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await policy.Execute(default, Wait, token);
      await policy.Execute(default, Wait, token);
    });
  }

  [Fact]
  public async Task ParallelMultiplyExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      ValueTask<Unit> first = policy.Execute(default, Wait, token);
      ValueTask<Unit> second = policy.Execute(default, Wait, token);
      await Task.WhenAll(first.AsTask(), second.AsTask());
    });
  }

  [Fact]
  public async Task ParallelNestedExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await policy.Execute(default, async (input, t) => {
        await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(input, Wait, t));
        return default;
      }, token);
    });
  }

  [Fact]
  public async Task NestedParallelExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, async (_, token) => {
      await Parallel.ForAsync(0, 10, token, async (_, t) => {
        await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(default, Wait, t));
      });
      return default;
    }, CancellationToken.None);
  }

  private async ValueTask<Unit> Wait(Unit input, CancellationToken cancellationToken = default) {
    await Task.Yield();
    return default;
  }

}