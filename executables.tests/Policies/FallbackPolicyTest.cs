using System.Runtime.ExceptionServices;
using Executables.Core.Policies;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(FallbackPolicy<,,>))]
public class FallbackPolicyTest {

  [Fact]
  public void RegularExecution() {
    IExecutor<Unit, Unit> executor = Executable
      .Identity()
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default));

    executor.Execute(default);
  }

  [Fact]
  public void ReturnFallbackOnException() {
    IExecutor<int, int> executor = Executable
      .Create<int, int>(int (_) => throw new InvalidOperationException())
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((input, _) => input));

    Assert.Equal(10, executor.Execute(10));
  }

  [Fact]
  public void ThrowExceptionFromFallbackHandler() {
    IExecutor<Unit, Unit> executor = Executable
      .Create<Unit, Unit>(Unit (_) => throw new InvalidOperationException())
      .GetExecutor()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default;
      }));

    Assert.Throws<InvalidOperationException>(() => executor.Execute(default));
  }

}