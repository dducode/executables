using Executables.Handling;
using JetBrains.Annotations;

namespace Executables.Tests.Handleables;

[TestSubject(typeof(HandleableExtensions))]
public class HandleableExtensionsTest {

  [Fact]
  public void PassNullAsyncHandleable() {
    Assert.Throws<ArgumentNullException>(() => AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler).Merge(null));
  }

  [Fact]
  public void PassNullHandleable() {
    Assert.Throws<ArgumentNullException>(() => Handleable.Create((Handler<Unit, Unit> handler) => handler).Merge(null));
  }

  [Fact]
  public void ThrowExceptionFromNullHandleable() {
    Assert.Throws<NullReferenceException>(() => ((Handleable<Unit, Unit>)null).Merge(null));
  }

  [Fact]
  public void ThrowExceptionFromNullAsyncHandleable() {
    Assert.Throws<NullReferenceException>(() => ((AsyncHandleable<Unit, Unit>)null).Merge(null));
  }

}