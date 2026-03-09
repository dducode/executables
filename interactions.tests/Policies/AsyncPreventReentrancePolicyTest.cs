using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncPreventReentrancePolicy<,>))]
public class AsyncPreventReentrancePolicyTest {

  [Fact]
  public async Task SequentialExecute() {
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(Executable.Identity().ToAsyncExecutable());

    await executable.Execute(default, CancellationToken.None);
    await executable.Execute(default, CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(Executable.Identity().ToAsyncExecutable());
    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await executable.Execute(default, cts.Token));
  }

  [Fact]
  public async Task MultiplyExecute() {
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(Executable.Identity().ToAsyncExecutable());

    ValueTask<Unit> first = executable.Execute(default, CancellationToken.None);
    ValueTask<Unit> second = executable.Execute(default, CancellationToken.None);
    await Task.WhenAll(first.AsTask(), second.AsTask());
  }

  [Fact]
  public async Task NestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(query);
    query.Handle(AsyncHandler.Create(async token => await executable.Execute(token)));

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Execute(default, CancellationToken.None));
  }

  [Fact]
  public async Task ParallelSequentialExecute() {
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(Executable.Identity().ToAsyncExecutable());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await executable.Execute(default, token);
      await executable.Execute(default, token);
    });
  }

  [Fact]
  public async Task ParallelMultiplyExecute() {
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(Executable.Identity().ToAsyncExecutable());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      ValueTask<Unit> first = executable.Execute(default, token);
      ValueTask<Unit> second = executable.Execute(default, token);
      await Task.WhenAll(first.AsTask(), second.AsTask());
    });
  }

  [Fact]
  public async Task ParallelNestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(query);
    query.Handle(AsyncHandler.Create(async token => await executable.Execute(token)));

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await Assert.ThrowsAsync<ReentranceException>(async () => await executable.Execute(default, token));
    });
  }

  [Fact]
  public async Task NestedParallelExecute() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutable<Unit, Unit> executable = AsyncPolicy.Of<Unit>().PreventReentrance().Apply(query);
    query.Handle(AsyncHandler.Create(async token => {
      await Parallel.ForAsync(0, 10, token, async (_, t) => await executable.Execute(t));
    }));

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Execute());
  }

}