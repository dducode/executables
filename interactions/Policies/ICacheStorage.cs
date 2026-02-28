namespace Interactions.Policies;

public interface ICacheStorage<in TKey, TValue> {

  void Add(TKey key, TValue value);
  bool TryGetValue(TKey key, out TValue value);

}