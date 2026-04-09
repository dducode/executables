using Executables.Lifecycle;
using JetBrains.Annotations;

namespace Executables.Tests.Disposables;

[TestSubject(typeof(DisposableBag))]
public class DisposableBagTest {

  [Fact]
  public void AddNullDisposable() {
    var bag = new DisposableBag();
    Assert.Throws<ArgumentNullException>(() => bag.Add(null));
  }

  [Fact]
  public void AddDisposableWhenDisposed() {
    var bag = new DisposableBag();
    bag.Dispose();
    Assert.Throws<ObjectDisposedException>(() => bag.Add(Disposable.Create(delegate { })));
  }

  [Fact]
  public void FirstDisposableThrowsException() {
    var secondDisposed = false;
    var bag = new DisposableBag();
    IDisposable first = Disposable.Create(() => throw new InvalidOperationException());
    IDisposable second = Disposable.Create(() => secondDisposed = true);
    bag.Add(first);
    bag.Add(second);
    Assert.Throws<InvalidOperationException>(() => bag.Dispose());
    Assert.True(secondDisposed);
  }

  [Fact]
  public void SecondDisposableThrowsException() {
    var bag = new DisposableBag();
    IDisposable first = Disposable.Create(delegate { });
    IDisposable second = Disposable.Create(() => throw new InvalidOperationException());
    bag.Add(first);
    bag.Add(second);
    Assert.Throws<InvalidOperationException>(() => bag.Dispose());
  }

  [Fact]
  public void BothDisposablesThrowsException() {
    var bag = new DisposableBag();
    IDisposable first = Disposable.Create(() => throw new InvalidOperationException());
    IDisposable second = Disposable.Create(() => throw new InvalidOperationException());
    bag.Add(first);
    bag.Add(second);
    Assert.Throws<AggregateException>(() => bag.Dispose());
  }

  [Fact]
  public void DoubleDispose() {
    var firstDisposed = false;
    var secondDisposed = false;
    var bag = new DisposableBag();

    bag.Add(Disposable.Create(() => {
      Assert.False(firstDisposed);
      firstDisposed = true;
    }));
    bag.Add(Disposable.Create(() => {
      Assert.False(secondDisposed);
      secondDisposed = true;
    }));

    bag.Dispose();
    bag.Dispose();
  }

}