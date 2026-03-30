using Executables.Core.Operators;
using Executables.Operations;
using JetBrains.Annotations;

namespace Executables.Tests.Operations;

[TestSubject(typeof(CacheOperator<,>))]
[TestSubject(typeof(AsyncCacheOperator<,>))]
public class CacheOperatorTest {

  [Fact]
  public void CacheValue() {
    var count = 0;

    IQuery<int, TimeSpan> query = Executable
      .Create((int seconds) => {
        count++;
        return TimeSpan.FromSeconds(seconds);
      })
      .Cache(new Cache())
      .AsQuery();

    Assert.Equal(TimeSpan.FromSeconds(10), query.Send(10));
    Assert.Equal(TimeSpan.FromSeconds(10), query.Send(10));
    Assert.Equal(1, count);

    Assert.Equal(TimeSpan.FromSeconds(20), query.Send(20));
    Assert.Equal(TimeSpan.FromSeconds(20), query.Send(20));
    Assert.Equal(2, count);
  }

  [Fact]
  public async Task AsyncCache() {
    var count = 0;

    IAsyncQuery<int, TimeSpan> query = AsyncExecutable
      .Create((int seconds, CancellationToken _) => {
        count++;
        return ValueTask.FromResult(TimeSpan.FromSeconds(seconds));
      })
      .Cache(new Cache())
      .AsQuery();

    Assert.Equal(TimeSpan.FromSeconds(10), await query.Send(10));
    Assert.Equal(TimeSpan.FromSeconds(10), await query.Send(10));
    Assert.Equal(1, count);

    Assert.Equal(TimeSpan.FromSeconds(20), await query.Send(20));
    Assert.Equal(TimeSpan.FromSeconds(20), await query.Send(20));
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