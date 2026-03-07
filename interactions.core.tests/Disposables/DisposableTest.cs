using Interactions.Core.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Disposables;

[TestSubject(typeof(Disposable))]
public class DisposableTest {

  [Fact]
  public void PassNullDisposable() {
    Assert.Throws<ArgumentNullException>(() => Disposable.Create(null));
  }

}