using Executables.Core.Executors;
using Executables.Operations;
using JetBrains.Annotations;

namespace Executables.Tests.Operations;

[TestSubject(typeof(CacheExecutor<,>))]
[TestSubject(typeof(AsyncCacheExecutor<,>))]
public class CacheExecutorTest {

  [Fact]
  public void CacheValue() {
    var count = 0;

    IExecutor<int, TimeSpan> executor = Executable
      .Create((int seconds) => {
        count++;
        return TimeSpan.FromSeconds(seconds);
      })
      .GetExecutor()
      .Cache(new Cache());

    Assert.Equal(TimeSpan.FromSeconds(10), executor.Execute(10));
    Assert.Equal(TimeSpan.FromSeconds(10), executor.Execute(10));
    Assert.Equal(1, count);

    Assert.Equal(TimeSpan.FromSeconds(20), executor.Execute(20));
    Assert.Equal(TimeSpan.FromSeconds(20), executor.Execute(20));
    Assert.Equal(2, count);
  }

  [Fact]
  public async Task AsyncCache() {
    var count = 0;

    IAsyncExecutor<int, TimeSpan> executor = AsyncExecutable
      .Create((int seconds, CancellationToken _) => {
        count++;
        return ValueTask.FromResult(TimeSpan.FromSeconds(seconds));
      })
      .GetExecutor()
      .Cache(new Cache());

    Assert.Equal(TimeSpan.FromSeconds(10), await executor.Execute(10));
    Assert.Equal(TimeSpan.FromSeconds(10), await executor.Execute(10));
    Assert.Equal(1, count);

    Assert.Equal(TimeSpan.FromSeconds(20), await executor.Execute(20));
    Assert.Equal(TimeSpan.FromSeconds(20), await executor.Execute(20));
    Assert.Equal(2, count);
  }

}

file class Cache : ICacheStorage<int, TimeSpan> {

  private readonly Dictionary<int, TimeSpan> _storage = new();

  public void Add(int key, TimeSpan value) {
    _storage.Add(key, value);
  }

  public bool TryGetValue(int key, out TimeSpan value) {
    return _storage.TryGetValue(key, out value);
  }

}