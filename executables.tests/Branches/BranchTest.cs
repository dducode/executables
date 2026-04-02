using Executables.Core.Executables;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Branches;

[TestSubject(typeof(WhenExecutable<,>))]
[TestSubject(typeof(OrWhenExecutable<,>))]
[TestSubject(typeof(OrElseExecutable<,>))]
public class BranchTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public void LinearBranch(int state, int expected) {
    IQuery<Unit, int> query = Executable
      .When(() => state == 0, () => 0)
      .OrWhen(() => state == 1, () => 1)
      .OrWhen(() => state == 2, () => 2)
      .OrElse(() => -1)
      .AsQuery();

    Assert.Equal(expected, query.Send());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public void NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    IQuery<Unit, int> query = Executable
      .When(() => topConditional, Executable
        .When(() => nestedConditional, () => 0)
        .OrElse(() => 1)
      ).OrElse(Executable
        .When(() => nestedConditional, () => 2)
        .OrElse(() => 3)
      ).AsQuery();

    Assert.Equal(expected, query.Send());
  }

  [Fact]
  public void PartialNestedBranch() {
    IQuery<int, Optional<int>> query = Executable
      .When(x => x > 0, Executable.When((int x) => x < 100, x => x * 2))
      .AsQuery();

    Assert.Equal(10, query.Send(5).Value);
    Assert.False(query.Send(0).HasValue);
    Assert.False(query.Send(100).HasValue);
  }

}