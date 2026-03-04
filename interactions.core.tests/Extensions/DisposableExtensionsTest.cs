using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Extensions;

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