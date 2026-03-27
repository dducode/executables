namespace Interactions;

/// <summary>
/// Represents an optional value that may be present or absent.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public readonly struct Optional<T>(T value) {

  /// <summary>
  /// Gets a value indicating whether an actual value is present.
  /// </summary>
  public bool HasValue { get; } = true;

  /// <summary>
  /// Gets the contained value or throws when no value is present.
  /// </summary>
  /// <exception cref="InvalidOperationException">Throws when no value is present.</exception>
  public T Value => HasValue ? value : throw new InvalidOperationException("Value not set");

  /// <summary>
  /// Gets the contained value or default value when no value is present.
  /// </summary>
  public T ValueOrDefault => value;

}