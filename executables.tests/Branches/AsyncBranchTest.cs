using Executables.Core.Executors;
using JetBrains.Annotations;

namespace Executables.Tests.Branches;

[TestSubject(typeof(AsyncWhenExecutor<,>))]
[TestSubject(typeof(AsyncOrWhenExecutor<,>))]
[TestSubject(typeof(AsyncOrElseExecutor<,>))]
public class AsyncBranchTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public async Task LinearBranch(int state, int expected) {
    IAsyncExecutor<Unit, int> executor = AsyncBranch
      .When(() => state == 0, AsyncExecutable.Create(_ => ValueTask.FromResult(0)))
      .OrWhen(() => state == 1, AsyncExecutable.Create(_ => ValueTask.FromResult(1)))
      .OrWhen(() => state == 2, AsyncExecutable.Create(_ => ValueTask.FromResult(2)))
      .OrElse(AsyncExecutable.Create(_ => ValueTask.FromResult(-1)));

    Assert.Equal(expected, await executor.Execute());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public async Task NestedBranch(bool topCondition, bool nestedCondition, int expected) {
    IAsyncExecutor<Unit, int> executor = AsyncBranch
      .When(() => topCondition, AsyncBranch
        .When(() => nestedCondition, AsyncExecutable.Create(_ => ValueTask.FromResult(0)))
        .OrElse(AsyncExecutable.Create(_ => ValueTask.FromResult(1)))
      ).OrElse(AsyncBranch
        .When(() => nestedCondition, AsyncExecutable.Create(_ => ValueTask.FromResult(2)))
        .OrElse(AsyncExecutable.Create(_ => ValueTask.FromResult(3)))
      );

    Assert.Equal(expected, await executor.Execute());
  }

}