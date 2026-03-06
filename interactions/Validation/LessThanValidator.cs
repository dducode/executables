using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class LessThanValidator<T>(T comparand, IComparer<T> comparer) : Validator<T> {

  public override string ErrorMessage { get; } = $"Value must be less than {comparand}";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return comparer.Compare(value, comparand) < 0;
  }

}