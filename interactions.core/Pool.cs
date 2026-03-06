namespace Interactions.Core;

internal static class Pool<T> where T : new() {

  private static readonly Stack<T> _pool = new();

  internal static T Get() {
    T list;
    lock (_pool)
      list = _pool.Count > 0 ? _pool.Pop() : new T();
    return list;
  }

  internal static void Return(T list) {
    lock (_pool)
      _pool.Push(list);
  }

}