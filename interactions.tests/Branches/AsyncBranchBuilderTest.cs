using Interactions.Branches;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Handlers;
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
    IAsyncExecutable<Unit, int> executable = Branch<int>
      .If(() => state == 0, _ => ValueTask.FromResult(0))
      .ElseIf(() => state == 1, _ => ValueTask.FromResult(1))
      .ElseIf(() => state == 2, _ => ValueTask.FromResult(2))
      .Else(_ => ValueTask.FromResult(-1));

    Assert.Equal(expected, await executable.Execute(default));
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public async Task NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    IAsyncExecutable<Unit, int> executable = Branch<int>
      .If(() => topConditional, Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(0))
        .Else(AsyncExecutable.Create(_ => ValueTask.FromResult(1)))
      ).Else(Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(2))
        .Else(_ => ValueTask.FromResult(3))
      );

    Assert.Equal(expected, await executable.Execute(default));
  }

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(null, Handler.Identity().ToAsyncHandler()));
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(() => true, (AsyncHandler<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Handler.Identity().ToAsyncHandler())
        .ElseIf(null, Handler.Identity().ToAsyncHandler());
    });
    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Handler.Identity().ToAsyncHandler())
        .ElseIf(() => true, null);
    });

    Assert.Throws<ArgumentNullException>(() => {
      IAsyncExecutable<Unit, Unit> _ = Branch<Unit, Unit>
        .If(() => true, Handler.Identity().ToAsyncHandler())
        .Else(null);
    });
  }

}