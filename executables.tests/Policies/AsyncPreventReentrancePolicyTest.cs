using Executables.Core.Policies;
using Executables.Policies;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(AsyncPreventReentrancePolicy<,>))]
public class AsyncPreventReentrancePolicyTest {

  [Fact]
  public async Task SequentialExecute() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    await executor.Execute();
    await executor.Execute();
  }

  [Fact]
  public async Task Cancel() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await executor.Execute(cts.Token));
  }

  [Fact]
  public async Task MultiplyExecute() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    ValueTask first = executor.Execute();
    ValueTask second = executor.Execute();
    await Task.WhenAll(first.AsTask(), second.AsTask());
  }

  [Fact]
  public async Task NestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutor<Unit, Unit> executor = query.GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    query.Handle(AsyncExecutable.Create(async token => await executor.Execute(token)).AsHandler());

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Send());
  }

  [Fact]
  public async Task ParallelSequentialExecute() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await executor.Execute(token);
      await executor.Execute(token);
    });
  }

  [Fact]
  public async Task ParallelMultiplyExecute() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      ValueTask first = executor.Execute(token);
      ValueTask second = executor.Execute(token);
      await Task.WhenAll(first.AsTask(), second.AsTask());
    });
  }

  [Fact]
  public async Task ParallelNestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutor<Unit, Unit> executor = query.GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    query.Handle(AsyncExecutable.Create(async token => await executor.Execute(token)).AsHandler());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await Assert.ThrowsAsync<ReentranceException>(async () => await executor.Execute(token));
    });
  }

  [Fact]
  public async Task NestedParallelExecute() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncExecutor<Unit, Unit> executor = query.GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    query.Handle(AsyncExecutable.Create(async token => {
      await Parallel.ForAsync(0, 10, token, async (_, t) => await executor.Execute(t));
    }).AsHandler());

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Send());
  }

}