using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(FallbackPolicy<,,>))]
public class FallbackPolicyTest {

  [Fact]
  public void RegularExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.Fallback<InvalidOperationException>(FallbackHandler);
    policy.Execute(default, Executable.Identity());
  }

  [Fact]
  public void ReturnFallbackOnException() {
    Policy<int, int> policy = Policy<int, int>.Fallback<InvalidOperationException>(FallbackHandler);
    Assert.Equal(10, policy.Execute(10, Executable.Create<int, int>(_ => throw new InvalidOperationException())));
  }

  [Fact]
  public void ThrowExceptionFromFallbackHandler() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.Fallback<InvalidOperationException>(RethrowHandler);
    Assert.Throws<InvalidOperationException>(() => policy.Execute(default, Executable.Create<Unit, Unit>(_ => throw new InvalidOperationException())));
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

}