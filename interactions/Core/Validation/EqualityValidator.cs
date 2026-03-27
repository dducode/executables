using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class EqualityValidator<T>(T expected, IEqualityComparer<T> equalityComparer) : Validator<T> {

  public override string ErrorMessage { get; } = $"Value must be equal to {expected}";

  public override bool IsValid(T value) {
    return equalityComparer.Equals(expected, value);
  }

}