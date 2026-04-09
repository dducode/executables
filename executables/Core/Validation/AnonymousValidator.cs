using Executables.Validation;

namespace Executables.Core.Validation;

internal sealed class AnonymousValidator<T>(Func<T, bool> validation, string errorMessage) : Validator<T> {

  public override string ErrorMessage { get; } = errorMessage;

  public override bool IsValid(T value) {
    return validation(value);
  }

}