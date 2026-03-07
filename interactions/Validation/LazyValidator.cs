using Interactions.Core;
using Interactions.Core.Resolvers;

namespace Interactions.Validation;

internal sealed class LazyValidator<T>(IResolver<Validator<T>> resolver) : Validator<T> {

  public override string ErrorMessage => _validator.Value.ErrorMessage;

  private readonly Core.Internal.Lazy<Validator<T>> _validator = new(resolver);

  public override bool IsValid(T value) {
    return _validator.Value.IsValid(value);
  }

}