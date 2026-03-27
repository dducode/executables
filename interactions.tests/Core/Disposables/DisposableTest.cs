using Interactions.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Tests.Core.Disposables;

[TestSubject(typeof(Disposable))]
public class DisposableTest {

  [Fact]
  public void PassNullDisposable() {
    Assert.Throws<ArgumentNullException>(() => Disposable.Create(null));
  }

}