namespace Interactions.Internal.Extensions;

internal static class CollectionExtensions {

  internal static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable) {
    foreach (T item in enumerable)
      collection.Add(item);
  }

}