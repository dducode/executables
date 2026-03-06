using Interactions.Core;

namespace Interactions.Validation;

internal sealed class LazyValidator<T>(IResolver<Validator<T>> resolver) : Validator<T> {

  public override string ErrorMessage => _validator.Value.ErrorMessage;

  private readonly Core.Lazy<Validator<T>> _validator = new(resolver);

  public override bool IsValid(T value) {
    return _validator.Value.IsValid(value);
  }

}