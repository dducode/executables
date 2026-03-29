using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(ForkExecutable<,,>))]
public class ForkExecutableTest {

  [Fact]
  public void SimpleFork() {
    IQuery<int, string> query = Executable
      .Identity<int>()
      .Fork(x => x * 2, x => x + 10)
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    Assert.Equal("20:20", query.Send(10));
  }

  [Fact]
  public void NestedFork() {
    IExecutable<int, int> branchSum = Executable
      .Identity<int>()
      .Fork(x => x * 2, x => x + 10)
      .Merge((a, b) => a + b);

    IExecutable<int, int> branchDiff = Executable
      .Identity<int>()
      .Fork(x => x * 2, x => x + 10)
      .Merge((a, b) => a - b);

    IQuery<int, string> query = Executable
      .Identity<int>()
      .Fork(branchSum, branchDiff)
      .Merge((a, b) => $"{a}:{b}")
      .AsQuery();

    Assert.Equal("40:0", query.Send(10));
  }

  [Fact]
  public void CompositionLaw() {
    IQuery<int, int> first = Executable
      .Identity<int>()
      .Fork(x => x * 2, x => x + 10)
      .Merge((a, b) => a + b)
      .AsQuery();

    IQuery<int, int> second = Executable
      .Identity<int>()
      .Then(x => x * 2 + x + 10)
      .AsQuery();

    const int value = 10;

    Assert.Equal(first.Send(value), second.Send(value));
  }

}