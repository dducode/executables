using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class OrValidator<T>(Validator<T> first, Validator<T> second) : Validator<T> {

  public override string ErrorMessage { get; } = $"{first.ErrorMessage} and {second.ErrorMessage}";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return first.IsValid(value) || second.IsValid(value);
  }

}