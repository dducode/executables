using Interactions.Branches;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using JetBrains.Annotations;

namespace Interactions.Tests.Branches;

[TestSubject(typeof(BranchBuilder<,>))]
public class BranchBuilderTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public void LinearBranch(int state, int expected) {
    IQuery<Unit, int> query = Branch<int>
      .If(() => state == 0, () => 0)
      .ElseIf(() => state == 1, () => 1)
      .ElseIf(() => state == 2, () => 2)
      .Else(() => -1)
      .AsQuery();

    Assert.Equal(expected, query.Send());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public void NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    IQuery<Unit, int> query = Branch<int>
      .If(() => topConditional, Branch<int>
        .If(() => nestedConditional, () => 0)
        .Else(() => 1)
      ).Else(Branch<int>
        .If(() => nestedConditional, () => 2)
        .Else(() => 3)
      ).AsQuery();

    Assert.Equal(expected, query.Send());
  }

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(null, Executable.Identity()));
    Assert.Throws<ArgumentNullException>(() => Branch<Unit, Unit>.If(_ => true, (IExecutable<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(_ => true, Executable.Identity())
        .ElseIf(null, Executable.Identity());
    });
    Assert.Throws<ArgumentNullException>(() => {
      Branch<Unit, Unit>
        .If(_ => true, Executable.Identity())
        .ElseIf(_ => true, null);
    });

    Assert.Throws<ArgumentNullException>(() => {
      IExecutable<Unit, Unit> _ = Branch<Unit, Unit>
        .If(_ => true, Executable.Identity())
        .Else(null);
    });
  }

}