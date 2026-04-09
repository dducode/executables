using System.Runtime.ExceptionServices;
using Executables.Core.Policies;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Identity()
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default));

    await executor.Execute();
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Identity()
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default));

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await executor.Execute(cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    IAsyncExecutor<int, int> executor = AsyncExecutable
      .Create<int, int>(ValueTask<int> (_, _) => throw new InvalidOperationException())
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((input, _) => input));

    Assert.Equal(10, await executor.Execute(10));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Create(ValueTask<Unit> (Unit _, CancellationToken _) => throw new InvalidOperationException())
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default;
      }));

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await executor.Execute());
  }

}