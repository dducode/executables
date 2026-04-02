using Executables.Core.Policies;
using Executables.Policies;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(AsyncPreventReentrancePolicy<,>))]
public class AsyncPreventReentrancePolicyTest {

  [Fact]
  public async Task SequentialExecute() {
    IAsyncQuery<Unit, Unit> query = Executable.Identity().ToAsyncExecutable().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    await query.Send();
    await query.Send();
  }

  [Fact]
  public async Task Cancel() {
    IAsyncQuery<Unit, Unit> query = Executable.Identity().ToAsyncExecutable().WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    var cts = new CancellationTokenSource();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(cts.Token));
  }

  [Fact]
  public async Task MultiplyExecute() {
    IAsyncQuery<Unit, Unit> query = Executable.Identity().ToAsyncExecutable().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    ValueTask first = query.Send();
    ValueTask second = query.Send();
    await Task.WhenAll(first.AsTask(), second.AsTask());
  }

  [Fact]
  public async Task NestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncQuery<Unit, Unit> inner = query.WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    query.Handle(AsyncExecutable.Create(async token => await inner.Send(token)).AsHandler());

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Send());
  }

  [Fact]
  public async Task ParallelSequentialExecute() {
    IAsyncQuery<Unit, Unit> query = Executable.Identity().ToAsyncExecutable().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await query.Send(token);
      await query.Send(token);
    });
  }

  [Fact]
  public async Task ParallelMultiplyExecute() {
    IAsyncQuery<Unit, Unit> query = Executable.Identity().ToAsyncExecutable().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    await Parallel.ForAsync(0, 10, async (_, token) => {
      ValueTask first = query.Send(token);
      ValueTask second = query.Send(token);
      await Task.WhenAll(first.AsTask(), second.AsTask());
    });
  }

  [Fact]
  public async Task ParallelNestedExecution() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncQuery<Unit, Unit> inner = query.WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    query.Handle(AsyncExecutable.Create(async token => await inner.Send(token)).AsHandler());

    await Parallel.ForAsync(0, 10, async (_, token) => {
      await Assert.ThrowsAsync<ReentranceException>(async () => await inner.Send(token));
    });
  }

  [Fact]
  public async Task NestedParallelExecute() {
    var query = new AsyncQuery<Unit, Unit>();
    IAsyncQuery<Unit, Unit> inner = query.WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    query.Handle(AsyncExecutable.Create(async token => {
      await Parallel.ForAsync(0, 10, token, async (_, t) => await inner.Send(t));
    }).AsHandler());

    await Assert.ThrowsAsync<ReentranceException>(async () => await query.Send());
  }

}