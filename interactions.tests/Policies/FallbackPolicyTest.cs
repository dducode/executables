using System.Runtime.ExceptionServices;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(FallbackPolicy<,,>))]
public class FallbackPolicyTest {

  [Fact]
  public void RegularExecution() {
    IQuery<Unit, Unit> query = Policy.Fallback((Unit _, InvalidOperationException _) => default(Unit))
      .Apply(Executable.Identity())
      .AsQuery();

    query.Send(default);
  }

  [Fact]
  public void ReturnFallbackOnException() {
    IQuery<int, int> query = Policy
      .Fallback((int input, InvalidOperationException _) => input)
      .Apply(Executable.Create<int, int>(_ => throw new InvalidOperationException()))
      .AsQuery();

    Assert.Equal(10, query.Send(10));
  }

  [Fact]
  public void ThrowExceptionFromFallbackHandler() {
    IQuery<Unit, Unit> query = Policy
      .Fallback((Unit _, InvalidOperationException ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default(Unit);
      })
      .Apply(Executable.Create<Unit, Unit>(_ => throw new InvalidOperationException()))
      .AsQuery();

    Assert.Throws<InvalidOperationException>(() => query.Send(default));
  }

}