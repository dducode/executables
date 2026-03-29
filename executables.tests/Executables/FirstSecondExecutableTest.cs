using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(FirstMapExecutable<,,>))]
[TestSubject(typeof(SecondMapExecutable<,,>))]
public class FirstSecondExecutableTest {

  [Fact]
  public void FirstSecond() {
    IQuery<int, int> query = Executable
      .Identity<int>()
      .Fork(x => x + 1, x => x + 2)
      .First(x => x * 10)
      .Second(x => x * 100)
      .Merge((a, b) => a + b)
      .AsQuery();

    Assert.Equal(320, query.Send(1));
  }

}