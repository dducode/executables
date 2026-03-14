using Interactions.Branches;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
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
    IAsyncQuery<Unit, int> query = Branch<int>
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
  public async Task NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    IAsyncQuery<Unit, int> query = Branch<int>
      .If(() => topConditional, Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(0))
        .Else(AsyncExecutable.Create(_ => ValueTask.FromResult(1)))
      ).Else(Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(2))
        .Else(_ => ValueTask.FromResult(3))
      ).AsQuery();

    Assert.Equal(expected, await query.Send());
  }

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(null, Executable.Identity().ToAsyncExecutable()));
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(() => true, (AsyncHandler<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Executable.Identity().ToAsyncExecutable())
        .ElseIf(null, Executable.Identity().ToAsyncExecutable());
    });
    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Executable.Identity().ToAsyncExecutable())
        .ElseIf(() => true, null);
    });

    Assert.Throws<ArgumentNullException>(() => {
      IAsyncExecutable<Unit, Unit> _ = Branch<Unit, Unit>
        .If(() => true, Executable.Identity().ToAsyncExecutable())
        .Else(null);
    });
  }

}