namespace Interactions.Core;

public static class ExceptionsHelper {

  public static void ThrowIfNull(object obj, string paramName) {
    if (obj == null)
      throw new ArgumentNullException(paramName);
  }

  public static void ThrowIfNullOrEmpty(string value, string paramName) {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException(paramName);
  }

  public static void ThrowIfEmpty<T>(ICollection<T> collection, string paramName) {
    if (collection.Count == 0)
      throw new ArgumentException(paramName);
  }

}