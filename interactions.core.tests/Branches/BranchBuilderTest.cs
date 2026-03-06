using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Branches;

[TestSubject(typeof(BranchBuilder<,>))]
public class BranchBuilderTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public void LinearBranch(int state, int expected) {
    Handler<Unit, int> handler = Branch<int>
      .If(() => state == 0, () => 0)
      .ElseIf(() => state == 1, () => 1)
      .ElseIf(() => state == 2, () => 2)
      .Else(() => -1);

    Assert.Equal(expected, handler.Execute(default));
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public void NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    Handler<Unit, int> handler = Branch<int>
      .If(() => topConditional, Branch<int>
        .If(() => nestedConditional, () => 0)
        .Else(() => 1)
      ).Else(Branch<int>
        .If(() => nestedConditional, () => 2)
        .Else(() => 3)
      );

    Assert.Equal(expected, handler.Execute(default));
  }

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(null, Handler.Identity()));
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(() => true, (Handler<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Handler.Identity())
        .ElseIf(null, Handler.Identity());
    });
    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(() => true, Handler.Identity())
        .ElseIf(() => true, null);
    });

    Assert.Throws<ArgumentNullException>(() => {
      Handler<Unit, Unit> _ = Branch<Unit, Unit>
        .If(() => true, Handler.Identity())
        .Else(null);
    });
  }

}