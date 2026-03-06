using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncFallbackPolicy<,,>))]
public class AsyncFallbackPolicyTest {

  [Fact]
  public async Task RegularExecution() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(FallbackHandler);
    await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), CancellationToken.None);
  }

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(FallbackHandler);

    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await policy.Execute(default, Executable.Identity().ToAsyncExecutable(), cts.Token));
  }

  [Fact]
  public async Task ReturnFallbackOnException() {
    AsyncPolicy<int, int> policy = AsyncPolicy<int, int>.Fallback<InvalidOperationException>(FallbackHandler);
    Assert.Equal(10, await policy.Execute(10, FailWait<int, int>(), CancellationToken.None));
  }

  [Fact]
  public async Task ThrowExceptionFromFallbackHandler() {
    AsyncPolicy<Unit, Unit> policy = AsyncPolicy<Unit, Unit>.Fallback<InvalidOperationException>(RethrowHandler);
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await policy.Execute(default, FailWait<Unit, Unit>(), CancellationToken.None));
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

  private IAsyncExecutable<T1, T2> FailWait<T1, T2>() {
    return AsyncExecutable.Create<T1, T2>((_, _) => throw new InvalidOperationException());
  }

}