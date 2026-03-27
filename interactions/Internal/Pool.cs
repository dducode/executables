namespace Interactions.Internal;

internal static class Pool<T> where T : new() {

  private static readonly Stack<T> _pool = new();

  internal static T Get() {
    T item;
    lock (_pool)
      item = _pool.Count > 0 ? _pool.Pop() : new T();
    return item;
  }

  internal static void Return(T item) {
    lock (_pool)
      _pool.Push(item);
  }

}