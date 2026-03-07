using Interactions.Core.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Disposables;

[TestSubject(typeof(DisposeHandle))]
public class DisposeHandleTest {

  [Fact]
  public void PassNullDisposable() {
    var handle = new DisposeHandle();
    Assert.Throws<ArgumentNullException>(() => handle.Register(null));
  }

  [Fact]
  public void RegisterDisposableWhenOtherExists() {
    var handle = new DisposeHandle();
    handle.Register(Disposable.Create(delegate { }));
    Assert.Throws<InvalidOperationException>(() => handle.Register(Disposable.Create(delegate { })));
  }

  [Fact]
  public void RegisterWhenHandleDisposed() {
    var handle = new DisposeHandle();
    handle.Dispose();
    Assert.Throws<ObjectDisposedException>(() => handle.Register(Disposable.Create(delegate { })));
  }

  [Fact]
  public void DoubleDispose() {
    var disposed = false;
    var handle = new DisposeHandle();

    handle.Register(Disposable.Create(() => {
      Assert.False(disposed);
      disposed = true;
    }));

    handle.Dispose();
    handle.Dispose();
  }

}