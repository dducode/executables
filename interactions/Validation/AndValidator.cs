using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class AndValidator<T>(Validator<T> left, Validator<T> right) : Validator<T> {

  public override string ErrorMessage { get; } = $"{left.ErrorMessage} or {right.ErrorMessage}";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return left.IsValid(value) && right.IsValid(value);
  }

}