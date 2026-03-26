using Interactions.Core;
using Interactions.Core.Executors;
using JetBrains.Annotations;

namespace Interactions.Tests.Extensions;

[TestSubject(typeof(ExecutorExtensions))]
public class CurriedExecutorTest {

  [Fact]
  public void CurryExecutor() {
    IExecutor<(int, int, int, int), string> executor = Executable.Create((int x, int y, int z, int w) => $"x: {x}, y: {y}, z: {z}, w: {w}").GetExecutor();
    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1).Execute(10).Execute(50).Execute(100));
    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1).Execute(10).Execute(50, 100));
    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1).Execute(10, 50).Execute(100));
    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1).Execute(10, 50, 100));

    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1, 10).Execute(50).Execute(100));
    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1, 10).Execute(50, 100));

    Assert.Equal(executor.Execute(1, 10, 50, 100), executor.Execute(1, 10, 50).Execute(100));
  }

  [Fact]
  public void SimpleOperation() {
    IExecutor<(int, int), int> operation = Executable.Create((int x, int y) => x + y).GetExecutor();
    IExecutor<int, int> step = operation.Execute(5);
    Assert.Equal(operation.Execute(5, 3), step.Execute(3));
    Assert.Equal(operation.Execute(5, 10), step.Execute(10));
  }

  [Fact]
  public void ComplicatedOperation() {
    IExecutor<(int, int, int), int> operation = Executable.Create((int x, int y, int z) => x * y + z).GetExecutor();
    IExecutor<(int, int), int> firstStep = operation.Execute(5);
    Assert.Equal(operation.Execute(5, 3, 10), firstStep.Execute(3, 10));
    Assert.Equal(operation.Execute(5, 10, 3), firstStep.Execute(10, 3));

    IExecutor<int, int> secondStep = firstStep.Execute(3);
    Assert.Equal(operation.Execute(5, 3, 10), secondStep.Execute(10));
    Assert.Equal(operation.Execute(5, 3, 100), secondStep.Execute(100));
  }

  [Fact]
  public async Task CurryAsyncExecutor() {
    IAsyncExecutor<(int, int, int, int), string> executor = AsyncExecutable.Create(async (int x, int y, int z, int w, CancellationToken token) => {
      await Task.Delay(10, token);
      return $"x: {x}, y: {y}, z: {z}, w: {w}";
    }).GetExecutor();

    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1).Execute(10).Execute(50).Execute(100));
    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1).Execute(10).Execute(50, 100));
    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1).Execute(10, 50).Execute(100));
    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1).Execute(10, 50, 100));

    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1, 10).Execute(50).Execute(100));
    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1, 10).Execute(50, 100));

    Assert.Equal(await executor.Execute(1, 10, 50, 100), await executor.Execute(1, 10, 50).Execute(100));
  }

}