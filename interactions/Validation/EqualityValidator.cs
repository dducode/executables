using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class EqualityValidator<T>(T expected, IEqualityComparer<T> equalityComparer) : Validator<T> {

  public override string ErrorMessage { get; } = $"Value must be equal to {expected}";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return equalityComparer.Equals(expected, value);
  }

}