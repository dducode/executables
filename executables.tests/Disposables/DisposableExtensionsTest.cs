using Executables.Lifecycle;
using JetBrains.Annotations;

namespace Executables.Tests.Disposables;

[TestSubject(typeof(DisposableExtensions))]
public class DisposableExtensionsTest {

  [Fact]
  public void ComposeWithNullDisposable() {
    Assert.Throws<ArgumentNullException>(() => Disposable.Create(delegate { }).Compose(null));
  }

  [Fact]
  public void ThrowExceptionFromNullDisposable() {
    Assert.Throws<NullReferenceException>(() => ((IDisposable)null).Compose(null));
  }

}