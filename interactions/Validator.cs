namespace Interactions;

/// <summary>
/// Represents a validation rule for values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Type being validated.</typeparam>
public abstract class Validator<T> {

  /// <summary>
  /// Gets the message associated with a validation failure.
  /// </summary>
  public abstract string ErrorMessage { get; }

  /// <summary>
  /// Determines whether the specified value satisfies the validator.
  /// </summary>
  /// <param name="value">Value to validate.</param>
  /// <returns><see langword="true"/> when the value is valid; otherwise <see langword="false"/>.</returns>
  public abstract bool IsValid(T value);

}