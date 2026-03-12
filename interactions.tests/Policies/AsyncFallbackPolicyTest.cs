using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    IAsyncExecutable<Unit> executable = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException _) => default(Unit))
      .Apply(Executable.Identity().ToAsyncExecutable());

    await executable.Execute(default, CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    IAsyncExecutable<Unit> executable = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException _) => default(Unit))
      .Apply(Executable.Identity().ToAsyncExecutable());

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await executable.Execute(default, cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    IAsyncExecutable<int, int> executable = AsyncPolicy
      .Fallback((int input, InvalidOperationException _) => input)
      .Apply(Executable.Create<int, int>(_ => throw new InvalidOperationException()).ToAsyncExecutable());

    Assert.Equal(10, await executable.Execute(10, CancellationToken.None));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    IAsyncExecutable<Unit> executable = AsyncPolicy
      .Fallback((Unit _, InvalidOperationException ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default(Unit);
      })
      .Apply(Executable.Create<Unit, Unit>(_ => throw new InvalidOperationException()).ToAsyncExecutable());

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await executable.Execute(default, CancellationToken.None));
  }

}