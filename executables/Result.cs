using System.Runtime.ExceptionServices;

namespace Executables;

/// <summary>
/// Represents either a successful value or a failure exception.
/// </summary>
/// <typeparam name="T">Successful value type.</typeparam>
public readonly struct Result<T> {

  /// <summary>
  /// Gets the successful value or throws when result is a failure.
  /// </summary>
  /// <exception cref="InvalidOperationException">Throws when result is a failure.</exception>
  public T Value => IsSuccess ? _value : throw new InvalidOperationException("Result is failure", Exception);

  /// <summary>
  /// Gets the failure exception, or <see langword="null"/> for successful results.
  /// </summary>
  public Exception Exception { get; }

  /// <summary>
  /// Gets a value indicating whether the result represents success.
  /// </summary>
  public bool IsSuccess => Exception == null;

  /// <summary>
  /// Gets a value indicating whether the result represents failure.
  /// </summary>
  public bool IsFailure => !IsSuccess;

  private readonly T _value;

  private Result(T value) {
    _value = value;
    Exception = null;
  }

  private Result(Exception exception) {
    _value = default;
    Exception = exception;
  }

  /// <summary>
  /// Creates a successful result from value.
  /// </summary>
  /// <param name="value">Successful value.</param>
  /// <returns>Successful result instance.</returns>
  public static Result<T> FromResult(T value) {
    return new Result<T>(value);
  }

  /// <summary>
  /// Creates a failed result from exception.
  /// </summary>
  /// <param name="exception">Failure exception.</param>
  /// <returns>Failed result instance.</returns>
  /// <exception cref="ArgumentNullException">Throws when <paramref name="exception"/> is <see langword="null"/>.</exception>
  public static Result<T> FromException(Exception exception) {
    return new Result<T>(exception ?? throw new ArgumentNullException(nameof(exception)));
  }

  /// <summary>
  /// Attempts to get successful value.
  /// </summary>
  /// <param name="value">When this method returns, contains the successful value or default value on failure.</param>
  /// <returns><see langword="true"/> for success; otherwise <see langword="false"/>.</returns>
  public bool TryGetValue(out T value) {
    value = _value;
    return IsSuccess;
  }

  /// <summary>
  /// Throws stored exception when result is a failure.
  /// </summary>
  public void ThrowIfFailure() {
    if (IsFailure)
      ExceptionDispatchInfo.Capture(Exception).Throw();
  }

  /// <summary>
  /// Returns textual representation of the value or failure exception.
  /// </summary>
  /// <returns>String representation of current result.</returns>
  public override string ToString() {
    return IsSuccess ? _value?.ToString() ?? "null" : Exception.ToString();
  }

}