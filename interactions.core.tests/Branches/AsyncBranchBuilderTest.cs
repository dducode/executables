using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Branches;

[TestSubject(typeof(AsyncBranchBuilder<,>))]
public class AsyncBranchBuilderTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public async Task LinearBranch(int state, int expected) {
    AsyncHandler<Unit, int> handler = Branch<int>
      .If(() => state == 0, _ => ValueTask.FromResult(0))
      .ElseIf(() => state == 1, _ => ValueTask.FromResult(1))
      .ElseIf(() => state == 2, _ => ValueTask.FromResult(2))
      .Else(_ => ValueTask.FromResult(-1));

    Assert.Equal(expected, await handler.Handle(default));
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public async Task NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    AsyncHandler<Unit, int> handler = Branch<int>
      .If(() => topConditional, Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(0))
        .Else(_ => ValueTask.FromResult(1))
      ).Else(Branch<int>
        .If(() => nestedConditional, _ => ValueTask.FromResult(2))
        .Else(_ => ValueTask.FromResult(3))
      );

    Assert.Equal(expected, await handler.Handle(default));
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
      AsyncHandler<Unit, Unit> _ = Branch<Unit, Unit>
        .If(() => true, Handler.Identity().ToAsyncHandler())
        .Else(null);
    });
  }

}