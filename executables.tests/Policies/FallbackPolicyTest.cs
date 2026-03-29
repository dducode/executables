using System.Runtime.ExceptionServices;
using Executables.Core.Policies;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(FallbackPolicy<,,>))]
public class FallbackPolicyTest {

  [Fact]
  public void RegularExecution() {
    IQuery<Unit, Unit> query = Executable
      .Identity()
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, _) => default))
      .AsQuery();

    query.Send(default);
  }

  [Fact]
  public void ReturnFallbackOnException() {
    IQuery<int, int> query = Executable
      .Create<int, int>(_ => throw new InvalidOperationException())
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((input, _) => input))
      .AsQuery();

    Assert.Equal(10, query.Send(10));
  }

  [Fact]
  public void ThrowExceptionFromFallbackHandler() {
    IQuery<Unit, Unit> query = Executable
      .Create<Unit, Unit>(_ => throw new InvalidOperationException())
      .WithPolicy(builder => builder.Fallback<InvalidOperationException>((_, ex) => {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return default;
      }))
      .AsQuery();

    Assert.Throws<InvalidOperationException>(() => query.Send(default));
  }

}