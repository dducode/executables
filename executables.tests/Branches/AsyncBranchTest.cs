using Executables.Core.Executables;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Branches;

[TestSubject(typeof(AsyncWhenExecutable<,>))]
[TestSubject(typeof(AsyncOrWhenExecutable<,>))]
[TestSubject(typeof(AsyncOrElseExecutable<,>))]
public class AsyncBranchTest {

  [Theory]
  [InlineData(0, 0)]
  [InlineData(1, 1)]
  [InlineData(2, 2)]
  [InlineData(3, -1)]
  public async Task LinearBranch(int state, int expected) {
    IAsyncQuery<Unit, int> query = AsyncExecutable
      .When(() => state == 0, _ => ValueTask.FromResult(0))
      .OrWhen(() => state == 1, _ => ValueTask.FromResult(1))
      .OrWhen(() => state == 2, _ => ValueTask.FromResult(2))
      .OrElse(_ => ValueTask.FromResult(-1))
      .AsQuery();

    Assert.Equal(expected, await query.Send());
  }

  [Theory]
  [InlineData(true, true, 0)]
  [InlineData(true, false, 1)]
  [InlineData(false, true, 2)]
  [InlineData(false, false, 3)]
  public async Task NestedBranch(bool topCondition, bool nestedCondition, int expected) {
    IAsyncQuery<Unit, int> query = AsyncExecutable
      .When(() => topCondition, AsyncExecutable
        .When(() => nestedCondition, _ => ValueTask.FromResult(0))
        .OrElse(_ => ValueTask.FromResult(1))
      ).OrElse(AsyncExecutable
        .When(() => nestedCondition, _ => ValueTask.FromResult(2))
        .OrElse(_ => ValueTask.FromResult(3))
      ).AsQuery();

    Assert.Equal(expected, await query.Send());
  }

}