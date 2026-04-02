namespace Executables;

/// <summary>
/// Represents an optional value that may be present or absent.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public readonly struct Optional<T>(T value) : IEquatable<Optional<T>> {

  private readonly T _value = value;

  /// <summary>
  /// Gets an empty optional value.
  /// </summary>
  public static Optional<T> None => default;

  /// <summary>
  /// Gets a value indicating whether an actual value is present.
  /// </summary>
  public bool HasValue { get; } = true;

  /// <summary>
  /// Gets the contained value or throws when no value is present.
  /// </summary>
  /// <exception cref="InvalidOperationException">Throws when no value is present.</exception>
  public T Value => HasValue ? _value : throw new InvalidOperationException("Value not set");

  /// <summary>
  /// Gets the contained value or default value when no value is present.
  /// </summary>
  public T ValueOrDefault => _value;

  /// <summary>
  /// Returns the string representation of the optional value.
  /// </summary>
  /// <returns>The contained value as a string, or <c>None</c> when no value is present.</returns>
  public override string ToString() {
    return HasValue ? Value?.ToString() ?? "Null" : "None";
  }

  public bool Equals(Optional<T> other) {
    if (!HasValue && !other.HasValue)
      return true;
    return HasValue == other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value);
  }

  public override bool Equals(object obj) {
    return obj is Optional<T> other && Equals(other);
  }

  public override int GetHashCode() {
    return HasValue ? EqualityComparer<T>.Default.GetHashCode(_value) : 0;
  }

  public static bool operator ==(Optional<T> left, Optional<T> right) {
    return left.Equals(right);
  }

  public static bool operator !=(Optional<T> left, Optional<T> right) {
    return !(left == right);
  }

}