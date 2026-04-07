using System.Diagnostics.Contracts;

namespace Executables.Context;

/// <summary>
/// Extension methods for working with executable contexts.
/// </summary>
public static class ExecutableContextExtensions {

  /// <summary>
  /// Stores a value in the context using its type as the key.
  /// </summary>
  public static void Set<T>(this ContextWriter context, T value) {
    context.Set(Key<T>(), value);
  }

  /// <summary>
  /// Gets a value from the current context or its ancestors.
  /// </summary>
  /// <exception cref="KeyNotFoundException">The specified key was not found.</exception>
  [Pure]
  public static T Get<T>(this IReadonlyContext context, object key) {
    return context.TryGet(key, out T value) ? value : throw new KeyNotFoundException();
  }

  /// <summary>
  /// Gets a value from the current context or its ancestors using its type as the key.
  /// </summary>
  /// <exception cref="KeyNotFoundException">The specified key was not found.</exception>
  [Pure]
  public static T Get<T>(this IReadonlyContext context) {
    return context.Get<T>(Key<T>());
  }

  /// <summary>
  /// Gets a value from the current context only.
  /// </summary>
  /// <exception cref="KeyNotFoundException">The specified key was not found.</exception>
  [Pure]
  public static T GetLocal<T>(this IReadonlyContext context, object key) {
    return context.TryGetLocal(key, out T value) ? value : throw new KeyNotFoundException();
  }

  /// <summary>
  /// Gets a value from the current context only using its type as the key.
  /// </summary>
  /// <exception cref="KeyNotFoundException">The specified key was not found.</exception>
  [Pure]
  public static T GetLocal<T>(this IReadonlyContext context) {
    return context.GetLocal<T>(Key<T>());
  }

  /// <summary>
  /// Tries to get a value from the current context or its ancestors using its type as the key.
  /// </summary>
  public static bool TryGet<T>(this IReadonlyContext context, out T value) {
    return context.TryGet(Key<T>(), out value);
  }

  /// <summary>
  /// Tries to get a value from the current context only using its type as the key.
  /// </summary>
  public static bool TryGetLocal<T>(this IReadonlyContext context, out T value) {
    return context.TryGetLocal(Key<T>(), out value);
  }

  /// <summary>
  /// Gets a value from the current context or its ancestors, or returns a default value.
  /// </summary>
  [Pure]
  public static T GetOrDefault<T>(this IReadonlyContext context, object key, T defaultValue = default) {
    return context.TryGet(key, out T value) ? value : defaultValue;
  }

  /// <summary>
  /// Gets a value from the current context or its ancestors using its type as the key, or returns a default value.
  /// </summary>
  [Pure]
  public static T GetOrDefault<T>(this IReadonlyContext context, T defaultValue = default) {
    return context.GetOrDefault(Key<T>(), defaultValue);
  }

  /// <summary>
  /// Gets a value from the current context only, or returns a default value.
  /// </summary>
  [Pure]
  public static T GetOrDefaultLocal<T>(this IReadonlyContext context, object key, T defaultValue = default) {
    return context.TryGetLocal(key, out T value) ? value : defaultValue;
  }

  /// <summary>
  /// Gets a value from the current context only using its type as the key, or returns a default value.
  /// </summary>
  [Pure]
  public static T GetOrDefaultLocal<T>(this IReadonlyContext context, T defaultValue = default) {
    return context.GetOrDefaultLocal(Key<T>(), defaultValue);
  }

  /// <summary>
  /// Determines whether the context contains the specified key.
  /// </summary>
  public static bool ContainsKey<T>(this IReadonlyContext context, T key) {
    return context.TryGet<T>(key, out _);
  }

  /// <summary>
  /// Determines whether the context contains a value keyed by type.
  /// </summary>
  public static bool ContainsKey<T>(this IReadonlyContext context) {
    return context.TryGet<T>(Key<T>(), out _);
  }

  private static object Key<T>() {
    return typeof(T);
  }

}