using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(CompositeExecutable<,,>))]
public class CompositeExecutableTest {

  [Fact]
  public void CompositionLaw() {
    IExecutable<int, int> f = Executable.Create((int x) => x * x);
    IExecutable<string, int> g = Executable.Create((string s) => int.Parse(s));

    IExecutor<string, int> first = f.Compose(g).GetExecutor();
    IExecutor<string, int> second = g.Then(f).GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      Assert.Equal(first.Execute(input), second.Execute(input));
    }
  }

  [Fact]
  public void AssociativeLaw() {
    IExecutable<string, int> a = Executable.Create((string s) => int.Parse(s));
    IExecutable<int, TimeSpan> b = Executable.Create((int x) => TimeSpan.FromSeconds(x));
    IExecutable<TimeSpan, string> c = Executable.Create((TimeSpan time) => time.ToString());

    IExecutor<string, string> directLeft = a.Then(b).Then(c).GetExecutor();
    IExecutor<string, string> directRight = a.Then(b.Then(c)).GetExecutor();
    IExecutor<string, string> reverseLeft = c.Compose(b).Compose(a).GetExecutor();
    IExecutor<string, string> reverseRight = c.Compose(b.Compose(a)).GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      string expected = directLeft.Execute(input);
      Assert.Equal(expected, directRight.Execute(input));
      Assert.Equal(expected, reverseLeft.Execute(input));
      Assert.Equal(expected, reverseRight.Execute(input));
    }
  }

  [Fact]
  public void DistributiveLaw() {
    IExecutable<int, int> f = Executable.Create((int x) => x + x);
    IExecutable<int, int> g = Executable.Create((int x) => x * x);
    IExecutable<string, int> h = Executable.Create((string s) => int.Parse(s));

    IExecutor<string, string> first = Executable.Fork(f, g).Compose(h)
      .Merge((x, y) => $"{x}:{y}")
      .GetExecutor();

    IExecutor<string, string> second = Executable.Fork(f.Compose(h), g.Compose(h))
      .Merge((x, y) => $"{x}:{y}")
      .GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      Assert.Equal(first.Execute(input), second.Execute(input));
    }
  }

  [Fact]
  public void IdentityLaw() {
    IExecutable<int, int> id = Executable.Identity<int>();
    IExecutable<int, int> f = Executable.Create((int x) => x * x);

    IExecutor<int, int> first = f.Then(id).GetExecutor();
    IExecutor<int, int> second = id.Then(f).GetExecutor();
    IExecutor<int, int> executor = f.GetExecutor();

    for (int i = -100; i < 100; i++) {
      Assert.Equal(executor.Execute(i), first.Execute(i));
      Assert.Equal(executor.Execute(i), second.Execute(i));
      Assert.Equal(first.Execute(i), second.Execute(i));
    }
  }

  [Fact]
  public void ComplicatedComposition() {
    IExecutor<int, int> executor = Executable
      .Create((int x) => x * x)
      .Compose((string s) => int.Parse(s))
      .Then(x => x.ToString())
      .Compose((int x) => x.ToString())
      .Then(int.Parse)
      .GetExecutor();

    Assert.Equal(25, executor.Execute(5));
  }

}