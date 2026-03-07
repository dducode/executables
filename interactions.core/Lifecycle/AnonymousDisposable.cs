namespace Interactions.Core.Lifecycle;

internal sealed class AnonymousDisposable(Action dispose) : IDisposable {

  public void Dispose() {
    dispose();
  }

}