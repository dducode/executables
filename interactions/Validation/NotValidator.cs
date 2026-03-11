namespace Interactions.Validation;

internal sealed class NotValidator<T>(Validator<T> inner) : Validator<T> {

  public override string ErrorMessage { get; } = $"NOT ({inner.ErrorMessage})";

  public override bool IsValid(T value) {
    return !inner.IsValid(value);
  }

}