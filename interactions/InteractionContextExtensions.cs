using System.Diagnostics.Contracts;

namespace Interactions;

public static class InteractionContextExtensions {

  public static void Set<T>(this InteractionContext context, T value) {
    context.Set(Key<T>(), value);
  }

  [Pure]
  public static T Get<T>(this IReadonlyContext context, object key) {
    return context.TryGet(key, out T value) ? value : throw new KeyNotFoundException();
  }

  [Pure]
  public static T Get<T>(this IReadonlyContext context) {
    return context.Get<T>(Key<T>());
  }

  [Pure]
  public static T GetLocal<T>(this IReadonlyContext context, object key) {
    return context.TryGetLocal(key, out T value) ? value : throw new KeyNotFoundException();
  }

  [Pure]
  public static T GetLocal<T>(this IReadonlyContext context) {
    return context.GetLocal<T>(Key<T>());
  }

  public static bool TryGet<T>(this IReadonlyContext context, out T value) {
    return context.TryGet(Key<T>(), out value);
  }

  public static bool TryGetLocal<T>(this IReadonlyContext context, out T value) {
    return context.TryGetLocal(Key<T>(), out value);
  }

  [Pure]
  public static T GetOrDefault<T>(this IReadonlyContext context, object key, T defaultValue = default) {
    return context.TryGet(key, out T value) ? value : defaultValue;
  }

  [Pure]
  public static T GetOrDefault<T>(this IReadonlyContext context, T defaultValue = default) {
    return context.GetOrDefault(Key<T>(), defaultValue);
  }

  [Pure]
  public static T GetOrDefaultLocal<T>(this IReadonlyContext context, object key, T defaultValue = default) {
    return context.TryGetLocal(key, out T value) ? value : defaultValue;
  }

  [Pure]
  public static T GetOrDefaultLocal<T>(this IReadonlyContext context, T defaultValue = default) {
    return context.GetOrDefaultLocal(Key<T>(), defaultValue);
  }

  private static object Key<T>() {
    return typeof(T);
  }

}