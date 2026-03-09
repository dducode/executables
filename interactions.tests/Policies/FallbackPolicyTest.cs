using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(FallbackPolicy<,,>))]
public class FallbackPolicyTest {

  [Fact]
  public void RegularExecution() {
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>().Fallback((Unit _, InvalidOperationException _) => default).Apply(Executable.Identity());
    executable.Execute(default);
  }

  [Fact]
  public void ReturnFallbackOnException() {
    IExecutable<int, int> executable = Policy.Of<int>()
      .Fallback((int input, InvalidOperationException _) => input)
      .Apply(Executable.Create<int, int>(_ => throw new InvalidOperationException()));
    Assert.Equal(10, executable.Execute(10));
  }

  [Fact]
  public void ThrowExceptionFromFallbackHandler() {
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>()
      .Fallback((Unit _, InvalidOperationException ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default;
      })
      .Apply(Executable.Create<Unit, Unit>(_ => throw new InvalidOperationException()));
    Assert.Throws<InvalidOperationException>(() => executable.Execute(default));
  }

}