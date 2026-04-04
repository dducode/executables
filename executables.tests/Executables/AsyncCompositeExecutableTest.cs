using Executables.Core.Executables;
using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(AsyncCompositeExecutable<,,>))]
public class AsyncCompositeExecutableTest {

  [Fact]
  public async Task CompositionLaw() {
    IAsyncExecutable<int, int> f = AsyncExecutable.Create(async (int x, CancellationToken _) => {
      await Task.Yield();
      return x * x;
    });

    IAsyncExecutable<string, int> g = AsyncExecutable.Create(async (string s, CancellationToken _) => {
      await Task.Yield();
      return int.Parse(s);
    });

    IAsyncExecutor<string, int> first = f.Compose(g).GetExecutor();
    IAsyncExecutor<string, int> second = g.Then(f).GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      Assert.Equal(await first.Execute(input), await second.Execute(input));
    }
  }

  [Fact]
  public async Task AssociativeLaw() {
    IAsyncExecutable<string, int> a = AsyncExecutable.Create(async (string s, CancellationToken _) => {
      await Task.Yield();
      return int.Parse(s);
    });

    IAsyncExecutable<int, TimeSpan> b = AsyncExecutable.Create(async (int x, CancellationToken _) => {
      await Task.Yield();
      return TimeSpan.FromSeconds(x);
    });

    IAsyncExecutable<TimeSpan, string> c = AsyncExecutable.Create(async (TimeSpan time, CancellationToken _) => {
      await Task.Yield();
      return time.ToString();
    });

    IAsyncExecutor<string, string> directLeft = a.Then(b).Then(c).GetExecutor();
    IAsyncExecutor<string, string> directRight = a.Then(b.Then(c)).GetExecutor();
    IAsyncExecutor<string, string> reverseLeft = c.Compose(b).Compose(a).GetExecutor();
    IAsyncExecutor<string, string> reverseRight = c.Compose(b.Compose(a)).GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      string expected = await directLeft.Execute(input);
      Assert.Equal(expected, await directRight.Execute(input));
      Assert.Equal(expected, await reverseLeft.Execute(input));
      Assert.Equal(expected, await reverseRight.Execute(input));
    }
  }

  [Fact]
  public async Task DistributiveLaw() {
    IAsyncExecutable<int, int> f = AsyncExecutable.Create(async (int x, CancellationToken _) => {
      await Task.Yield();
      return x + x;
    });

    IAsyncExecutable<int, int> g = AsyncExecutable.Create(async (int x, CancellationToken _) => {
      await Task.Yield();
      return x * x;
    });

    IAsyncExecutable<string, int> h = AsyncExecutable.Create(async (string s, CancellationToken _) => {
      await Task.Yield();
      return int.Parse(s);
    });

    IAsyncExecutor<string, string> first = AsyncExecutable.Fork(f, g).Compose(h)
      .Merge((x, y) => $"{x}:{y}")
      .GetExecutor();

    IAsyncExecutor<string, string> second = AsyncExecutable.Fork(f.Compose(h), g.Compose(h))
      .Merge((x, y) => $"{x}:{y}")
      .GetExecutor();

    for (int i = -100; i < 100; i++) {
      var input = i.ToString();
      Assert.Equal(await first.Execute(input), await second.Execute(input));
    }
  }

  [Fact]
  public async Task IdentityLaw() {
    IAsyncExecutable<int, int> id = AsyncExecutable.Identity<int>();

    IAsyncExecutable<int, int> f = AsyncExecutable.Create(async (int x, CancellationToken _) => {
      await Task.Yield();
      return x * x;
    });

    IAsyncExecutor<int, int> first = f.Then(id).GetExecutor();
    IAsyncExecutor<int, int> second = id.Then(f).GetExecutor();
    IAsyncExecutor<int, int> executor = f.GetExecutor();

    for (int i = -100; i < 100; i++) {
      Assert.Equal(await executor.Execute(i), await first.Execute(i));
      Assert.Equal(await executor.Execute(i), await second.Execute(i));
      Assert.Equal(await first.Execute(i), await second.Execute(i));
    }
  }

}