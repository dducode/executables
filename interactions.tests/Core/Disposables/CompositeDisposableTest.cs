using Interactions.Core.Lifecycle;
using Interactions.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Tests.Core.Disposables;

[TestSubject(typeof(CompositeDisposable))]
public class CompositeDisposableTest {

  [Fact]
  public void FirstDisposableThrowsException() {
    var secondDisposed = false;
    IDisposable first = Disposable.Create(() => throw new InvalidOperationException());
    IDisposable second = Disposable.Create(() => secondDisposed = true);
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Dispose());
    Assert.True(secondDisposed);
  }

  [Fact]
  public void SecondDisposableThrowsException() {
    IDisposable first = Disposable.Create(delegate { });
    IDisposable second = Disposable.Create(() => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Dispose());
  }

  [Fact]
  public void BothDisposablesThrowsException() {
    IDisposable first = Disposable.Create(() => throw new InvalidOperationException());
    IDisposable second = Disposable.Create(() => throw new InvalidOperationException());
    Assert.Throws<AggregateException>(() => first.Compose(second).Dispose());
  }

  [Fact]
  public void DoubleDispose() {
    var firstDisposed = false;
    var secondDisposed = false;

    IDisposable disposable = Disposable.Create(() => {
      Assert.False(firstDisposed);
      firstDisposed = true;
    }).Compose(Disposable.Create(() => {
      Assert.False(secondDisposed);
      secondDisposed = true;
    }));

    disposable.Dispose();
    disposable.Dispose();
  }

}