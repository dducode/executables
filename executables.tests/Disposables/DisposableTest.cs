using Executables.Lifecycle;
using JetBrains.Annotations;

namespace Executables.Tests.Disposables;

[TestSubject(typeof(Disposable))]
public class DisposableTest {

  [Fact]
  public void PassNullDisposable() {
    Assert.Throws<ArgumentNullException>(() => Disposable.Create(null));
  }

}