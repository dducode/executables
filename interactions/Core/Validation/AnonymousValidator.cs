using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class AnonymousValidator<T>(Func<T, bool> validation, string errorMessage) : Validator<T> {

  public override string ErrorMessage { get; } = errorMessage;

  public override bool IsValid(T value) {
    return validation(value);
  }

}