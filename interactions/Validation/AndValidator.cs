namespace Interactions.Validation;

internal sealed class AndValidator<T>(Validator<T> left, Validator<T> right) : Validator<T> {

  public override string ErrorMessage { get; } = $"{left.ErrorMessage} or {right.ErrorMessage}";

  public override bool IsValid(T value) {
    return left.IsValid(value) && right.IsValid(value);
  }

}