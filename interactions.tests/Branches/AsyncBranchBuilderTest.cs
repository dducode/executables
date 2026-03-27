using Interactions.Branches;
using Interactions.Queries;
using JetBrains.Annotations;

namespace Interactions.Tests.Branches;

[TestSubject(typeof(AsyncBranchBuilder<,>))]
public class AsyncBranchBuilderTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public async Task LinearBranch(int state, int expected) {
    IAsyncQuery<Unit, int> query = AsyncBranch<int>
      .If(() => state == 0, _ => ValueTask.FromResult(0))
      .ElseIf(() => state == 1, _ => ValueTask.FromResult(1))
      .ElseIf(() => state == 2, _ => ValueTask.FromResult(2))
      .Else(_ => ValueTask.FromResult(-1))
      .AsQuery();

    Assert.Equal(expected, await query.Send());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public async Task NestedBranch(bool topCondition, bool nestedCondition, int expected) {
    IAsyncQuery<Unit, int> query = AsyncBranch<int>
      .If(() => topCondition, AsyncBranch<int>
        .If(() => nestedCondition, _ => ValueTask.FromResult(0))
        .Else(AsyncExecutable.Create(_ => ValueTask.FromResult(1)))
      ).Else(AsyncBranch<int>
        .If(() => nestedCondition, _ => ValueTask.FromResult(2))
        .Else(_ => ValueTask.FromResult(3))
      ).AsQuery();

    Assert.Equal(expected, await query.Send());
  }

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => AsyncBranch<Unit, Unit>.If(null, AsyncExecutable.Identity()));
    Assert.Throws<ArgumentNullException>(() => AsyncBranch<Unit, Unit>.If(_ => true, (IAsyncExecutable<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => {
      AsyncBranch<Unit, Unit>
        .If(_ => true, AsyncExecutable.Identity())
        .ElseIf(null, AsyncExecutable.Identity());
    });
    Assert.Throws<ArgumentNullException>(() => {
      AsyncBranch<Unit, Unit>
        .If(_ => true, AsyncExecutable.Identity())
        .ElseIf(_ => true, null);
    });

    Assert.Throws<ArgumentNullException>(() => {
      IAsyncExecutable<Unit, Unit> _ = AsyncBranch<Unit, Unit>
        .If(_ => true, AsyncExecutable.Identity())
        .Else(null);
    });
  }

}