using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(FallbackHandler);
    await policy.Execute(default, Wait, CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(FallbackHandler);

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await policy.Execute(default, Wait, cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    AsyncPolicy<int, int> policy = AsyncPolicy<int, int>.Fallback<InvalidOperationException>(FallbackHandler);
    Assert.Equal(10, await policy.Execute(10, FailWait, CancellationToken.None));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(RethrowHandler);
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await policy.Execute(default, FailWait, CancellationToken.None));
  }

  private Unit FallbackHandler<TEx>(Unit input, TEx ex) where TEx : Exception {
    return default;
  }

  private int FallbackHandler<TEx>(int input, TEx ex) where TEx : Exception {
    return input;
  }

  private Unit RethrowHandler<TEx>(Unit input, TEx ex) where TEx : Exception {
    ExceptionDispatchInfo.Capture(ex).Throw();
    return default;
  }

  private async ValueTask<Unit> Wait(Unit input, CancellationToken token) {
    await Task.Yield();
    return default;
  }

  private ValueTask<T> FailWait<T>(T input, CancellationToken token) {
    throw new InvalidOperationException();
  }

}