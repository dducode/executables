using JetBrains.Annotations;

namespace Executables.Tests.Pipes;

[TestSubject(typeof(Pipe))]
public class PipeTest {

  [Fact]
  public void ForkMergePipe() {
    IPipe<IExecutable<string, int>, IExecutor<string, string>> executablePipe = Pipe.Create((IExecutable<string, int> executable) => executable
      .Fork(x => x + x, x => x * x)
      .Swap()
      .First(y => y.ToString())
      .Second(x => x.ToString())
      .Merge((x, y) => $"{x}:{y}")
      .GetExecutor()
    );

    IExecutable<string, int> executable = Executable.Create((string s) => int.Parse(s));
    IExecutor<string, string> applied = executablePipe.Apply(executable);
    Assert.Equal("100:20", applied.Execute("10"));
  }

}