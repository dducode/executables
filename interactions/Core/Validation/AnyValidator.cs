using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class AnyValidator<T>(Validator<T> itemValidator) : Validator<IEnumerable<T>> {

  public override string ErrorMessage { get; } = $"At least one element must satisfy: {itemValidator.ErrorMessage}";

  public override bool IsValid(IEnumerable<T> value) {
    return value.Any(itemValidator.IsValid);
  }

}