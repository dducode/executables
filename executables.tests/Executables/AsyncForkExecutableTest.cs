using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(AsyncForkExecutable<,,>))]
public class AsyncForkExecutableTest {

  [Fact]
  public async Task SimpleFork() {
    IAsyncQuery<int, string> query = AsyncExecutable
      .Identity<int>()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        })
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    Assert.Equal("2:11", await query.Send(1));
  }

  [Fact]
  public async Task NestedFork() {
    IAsyncExecutable<int, int> branchSum = AsyncExecutable
      .Identity<int>()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        })
      .Merge((a, b) => a + b);

    IAsyncExecutable<int, int> branchDiff = AsyncExecutable
      .Identity<int>()
      .Fork(
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        },
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        })
      .Merge((a, b) => a - b);

    IAsyncQuery<int, string> query = AsyncExecutable
      .Identity<int>()
      .Fork(branchSum, branchDiff)
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    Assert.Equal("13:9", await query.Send(1));
  }

  [Fact]
  public async Task ThrowExceptionWhenAnyFaulted() {
    IAsyncQuery<int, string> firstFaulted = AsyncExecutable
      .Identity<int>()
      .Fork(
        ValueTask<int> (_, _) => throw new InvalidOperationException(),
        async (x, token) => {
          await Task.Delay(10, token);
          return x + 2;
        })
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    IAsyncQuery<int, string> secondFaulted = AsyncExecutable
      .Identity<int>()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x + 2;
        },
        ValueTask<int> (_, _) => throw new InvalidOperationException())
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    IAsyncQuery<int, string> bothFaulted = AsyncExecutable
      .Identity<int>()
      .Fork(
        ValueTask<int> (_, _) => throw new InvalidOperationException(),
        ValueTask<int> (_, _) => throw new InvalidOperationException())
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await firstFaulted.Send(0));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await secondFaulted.Send(0));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await bothFaulted.Send(0));
  }

}