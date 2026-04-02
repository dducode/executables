using System.Runtime.ExceptionServices;
using Executables.Core.Policies;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    IAsyncQuery<Unit, Unit> query = Executable
      .Identity()
      .ToAsyncExecutable()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default))
      .AsQuery();

    await query.Send();
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    IAsyncQuery<Unit, Unit> query = Executable
      .Identity()
      .ToAsyncExecutable()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default))
      .AsQuery();

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Create<int, int>((_, _) => throw new InvalidOperationException())
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((input, _) => input))
      .AsQuery();

    Assert.Equal(10, await query.Send(10));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create<Unit, Unit>((_, _) => throw new InvalidOperationException())
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default;
      }))
      .AsQuery();

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await query.Send());
  }

}