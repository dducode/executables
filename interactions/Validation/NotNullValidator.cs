namespace Interactions.Validation;

internal sealed class NotNullValidator<T> : Validator<T> {

  internal static NotNullValidator<T> Instance { get; } = new();
  public override string ErrorMessage => "Value cannot be null";

  private NotNullValidator() {
  }

  public override bool IsValid(T value) {
    return value != null;
  }

}