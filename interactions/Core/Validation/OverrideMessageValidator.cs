using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class OverrideMessageValidator<T>(Validator<T> inner, string errorMessage) : Validator<T> {

  public override string ErrorMessage { get; } = errorMessage;

  public override bool IsValid(T value) {
    return inner.IsValid(value);
  }

}