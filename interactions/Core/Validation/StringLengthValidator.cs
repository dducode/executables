using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class StringLengthValidator(Validator<int> lengthValidator) : Validator<string> {

  public override string ErrorMessage { get; } = lengthValidator.ErrorMessage;

  public override bool IsValid(string value) {
    return lengthValidator.IsValid(value.Length);
  }

}