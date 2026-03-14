using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    IAsyncQuery<Unit, Unit> query = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException _) => default(Unit))
      .Apply(Executable.Identity().ToAsyncExecutable())
      .AsQuery();

    await query.Send();
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    IAsyncQuery<Unit, Unit> query = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException _) => default(Unit))
      .Apply(Executable.Identity().ToAsyncExecutable())
      .AsQuery();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    IAsyncQuery<int, int> query = AsyncPolicy
      .Fallback((int input, InvalidOperationException _) => input)
      .Apply(Executable.Create<int, int>(_ => throw new InvalidOperationException()).ToAsyncExecutable())
      .AsQuery();

    Assert.Equal(10, await query.Send(10));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    IAsyncQuery<Unit, Unit> query = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default(Unit);
      })
      .Apply(Executable.Create<Unit, Unit>(_ => throw new InvalidOperationException()).ToAsyncExecutable())
      .AsQuery();

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await query.Send());
  }

}