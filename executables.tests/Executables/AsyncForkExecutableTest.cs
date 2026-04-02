using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(AsyncForkExecutable<,,>))]
public class AsyncForkExecutableTest {

  [Fact]
  public async Task SimpleFork() {
    IAsyncQuery<int, string> query = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        })
      .Merge((a, b, _) => new ValueTask<string>($"{a}:{b}"))
      .AsQuery();

    Assert.Equal("2:11", await query.Send(1));
  }

  [Fact]
  public async Task NestedFork() {
    IAsyncExecutable<int, int> branchSum = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        })
      .Merge((a, b, _) => new ValueTask<int>(a + b));

    IAsyncExecutable<int, int> branchDiff = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        },
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        })
      .Merge((a, b, _) => new ValueTask<int>(a - b));

    IAsyncQuery<int, string> query = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(branchSum, branchDiff)
      .Merge((a, b, _) => new ValueTask<string>($"{a}:{b}"))
      .AsQuery();

    Assert.Equal("13:9", await query.Send(1));
  }

  [Fact]
  public async Task ThrowExceptionWhenAnyFaulted() {
    IAsyncQuery<int, string> firstFaulted = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        ValueTask<int> (_, _) => throw new InvalidOperationException(),
        async (x, token) => {
          await Task.Delay(10, token);
          return x + 2;
        })
      .Merge((a, b, _) => new ValueTask<string>($"{a}:{b}"))
      .AsQuery();

    IAsyncQuery<int, string> secondFaulted = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        async (x, token) => {
          await Task.Delay(10, token);
          return x + 2;
        },
        ValueTask<int> (_, _) => throw new InvalidOperationException())
      .Merge((a, b, _) => new ValueTask<string>($"{a}:{b}"))
      .AsQuery();

    IAsyncQuery<int, string> bothFaulted = Executable
      .Identity<int>()
      .ToAsyncExecutable()
      .Fork(
        ValueTask<int> (_, _) => throw new InvalidOperationException(),
        ValueTask<int> (_, _) => throw new InvalidOperationException())
      .Merge((a, b, _) => new ValueTask<string>($"{a}:{b}"))
      .AsQuery();

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await firstFaulted.Send(0));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await secondFaulted.Send(0));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await bothFaulted.Send(0));
  }

}