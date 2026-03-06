using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncPreventReentrancePolicy<,>))]
public class AsyncPreventReentrancePolicyTest {

  [Fact]
  public async Task SequentialExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), CancellationToken.None);
    await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();
    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), cts.Token));
  }

  [Fact]
  public async Task MultiplyExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    ValueTask<Unit> first = policy.Execute(default, Executable.Identity().ToAsyncExecutable(), CancellationToken.None);
    ValueTask<Unit> second = policy.Execute(default, Executable.Identity().ToAsyncExecutable(), CancellationToken.None);
    await Task.WhenAll(first.AsTask(), second.AsTask());
  }

  [Fact]
  public async Task NestedExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, Executable.CreateAsync(async (Unit input, CancellationToken token) => {
      await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(input, Executable.Identity().ToAsyncExecutable(), token));
    }), CancellationToken.None);
  }

  [Fact]
  public async Task ParallelSequentialExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), token);
      await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), token);
    });
  }

  [Fact]
  public async Task ParallelMultiplyExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      ValueTask<Unit> first = policy.Execute(default, Executable.Identity().ToAsyncExecutable(), token);
      ValueTask<Unit> second = policy.Execute(default, Executable.Identity().ToAsyncExecutable(), token);
      await Task.WhenAll(first.AsTask(), second.AsTask());
    });
  }

  [Fact]
  public async Task ParallelNestedExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await policy.Execute(default, Executable.CreateAsync(async (Unit input, CancellationToken t) => {
        await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(input, Executable.Identity().ToAsyncExecutable(), t));
      }), token);
    });
  }

  [Fact]
  public async Task NestedParallelExecute() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.PreventReentrance();

    await policy.Execute(default, Executable.CreateAsync(async (Unit _, CancellationToken token) => {
      await Parallel.ForAsync(0, 10, token, async (_, t) => {
        await Assert.ThrowsAsync<ReentranceException>(async () => await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), t));
      });
    }), CancellationToken.None);
  }

}