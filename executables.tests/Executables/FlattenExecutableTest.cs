using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(FlattenExecutable<,>))]
public class FlattenExecutableTest {

  [Fact]
  public void SimpleTest() {
    IExecutable<int, int> sum = Executable.Create((int x) => x + x);
    IExecutable<int, int> fallback = Executable.Create((int _) => 0);
    IQuery<int, int> query = Executable
      .FlatMap((int x) => x > 0 ? sum : fallback)
      .AsQuery();

    Assert.Equal(10, query.Send(5));
    Assert.Equal(0, query.Send(0));
    Assert.Equal(0, query.Send(-5));
  }

}