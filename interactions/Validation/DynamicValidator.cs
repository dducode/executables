using Interactions.Core;
using Interactions.Core.Providers;

namespace Interactions.Validation;

internal sealed class DynamicValidator<T>(IProvider<Validator<T>> provider) : Validator<T> {

  public override string ErrorMessage => _errorMessage;
  private string _errorMessage;

  public override bool IsValid(T value) {
    Validator<T> validator = provider.Get();
    _errorMessage = validator?.ErrorMessage;
    return validator?.IsValid(value) ?? throw new InvalidOperationException($"Cannot resolve validator by {provider.GetType().Name}");
  }

}