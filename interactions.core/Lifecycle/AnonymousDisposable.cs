namespace Interactions.Core.Lifecycle;

internal sealed class AnonymousDisposable(Action dispose) : IDisposable {

  void IDisposable.Dispose() {
    dispose();
  }

}