namespace Interactions.Internal;

internal readonly struct ListHandle<T>(List<T> list) : IDisposable {

  public void Dispose() {
    list.Clear();
    Pool<List<T>>.Return(list);
  }

}