namespace Executables.Operations;

/// <summary>
/// Defines key-value storage used by cache operator.
/// </summary>
/// <typeparam name="TKey">Cache key type.</typeparam>
/// <typeparam name="TValue">Cached value type.</typeparam>
public interface ICacheStorage<in TKey, TValue> {

  /// <summary>
  /// Adds a value associated with the specified key.
  /// </summary>
  /// <param name="key">Cache key.</param>
  /// <param name="value">Value to store.</param>
  void Add(TKey key, TValue value);

  /// <summary>
  /// Tries to get a cached value by key.
  /// </summary>
  /// <param name="key">Cache key.</param>
  /// <param name="value">When this method returns, contains the cached value if found; otherwise default value.</param>
  /// <returns><see langword="true"/> when a value is found; otherwise <see langword="false"/>.</returns>
  bool TryGetValue(TKey key, out TValue value);

}