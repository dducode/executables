using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class NotValidator<T>(Validator<T> inner) : Validator<T> {

  public override string ErrorMessage { get; } = $"NOT ({inner.ErrorMessage})";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return !inner.IsValid(value);
  }

}