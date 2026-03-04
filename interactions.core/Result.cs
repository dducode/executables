using System.Runtime.ExceptionServices;

namespace Interactions.Core;

public readonly struct Result<T> {

  private const string ErrorMessage = "Result is in invalid state. You cannot return Result as default, only from valid value or exception";

  public T Value {
    get {
      if (_isSuccess)
        return _value;

      ExceptionDispatchInfo.Capture(_exception ?? throw new InvalidOperationException(ErrorMessage)).Throw();
      return default;
    }
  }

  public Exception Exception => _isValid ? _exception : throw new InvalidOperationException(ErrorMessage);

  public bool IsSuccess => _isValid ? _isSuccess : throw new InvalidOperationException(ErrorMessage);
  public bool IsFailure => !IsSuccess;
  public bool IsValid => _isValid;

  private readonly bool _isSuccess;
  private readonly T _value;
  private readonly Exception _exception;
  private readonly bool _isValid;

  private Result(T value) {
    _isSuccess = true;
    _value = value;
    _exception = null;
    _isValid = true;
  }

  private Result(Exception exception) {
    _isSuccess = false;
    _value = default;
    _exception = exception;
    _isValid = true;
  }

  public static implicit operator Result<T>(T value) {
    return new Result<T>(value);
  }

  public static Result<T> FromResult(T value) {
    return new Result<T>(value);
  }

  public static Result<T> FromException(Exception exception) {
    return new Result<T>(exception ?? throw new ArgumentNullException(nameof(exception)));
  }

  public bool TryGetValue(out T value) {
    if (!_isValid)
      throw new InvalidOperationException(ErrorMessage);

    value = _value;
    return _isSuccess;
  }

  public override string ToString() {
    return _isSuccess ? _value?.ToString() ?? "null" : Exception?.Message ?? throw new InvalidOperationException(ErrorMessage);
  }

}