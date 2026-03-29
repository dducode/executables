using Executables.Validation;

namespace Executables.Core.Validation;

internal sealed class AllValidator<T>(Validator<T> itemValidator) : Validator<IEnumerable<T>> {

  public override string ErrorMessage { get; } = $"All elements must satisfy: {itemValidator.ErrorMessage}";

  public override bool IsValid(IEnumerable<T> value) {
    return value.All(itemValidator.IsValid);
  }

}