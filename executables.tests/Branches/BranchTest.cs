using Executables.Core.Executors;
using JetBrains.Annotations;

namespace Executables.Tests.Branches;

[TestSubject(typeof(WhenExecutor<,>))]
[TestSubject(typeof(OrWhenExecutor<,>))]
[TestSubject(typeof(OrElseExecutor<,>))]
public class BranchTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public void LinearBranch(int state, int expected) {
    IExecutor<Unit, int> executor = Branch
      .When(() => state == 0, Executable.Create(() => 0))
      .OrWhen(() => state == 1, Executable.Create(() => 1))
      .OrWhen(() => state == 2, Executable.Create(() => 2))
      .OrElse(Executable.Create(() => -1));

    Assert.Equal(expected, executor.Execute());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public void NestedBranch(bool topConditional, bool nestedConditional, int expected) {
    IExecutor<Unit, int> executor = Branch
      .When(() => topConditional, Branch
        .When(() => nestedConditional, Executable.Create(() => 0))
        .OrElse(Executable.Create(() => 1))
      ).OrElse(Branch
        .When(() => nestedConditional, Executable.Create(() => 2))
        .OrElse(Executable.Create(() => 3))
      );

    Assert.Equal(expected, executor.Execute());
  }

  [Fact]
  public void PartialNestedBranch() {
    IExecutor<int, Optional<int>> executor = Branch
      .When(x => x > 0, Branch.When(x => x < 100, Executable.Create((int x) => x * 2)));

    Assert.Equal(10, executor.Execute(5).Value);
    Assert.False(executor.Execute(0).HasValue);
    Assert.False(executor.Execute(100).HasValue);
  }

}