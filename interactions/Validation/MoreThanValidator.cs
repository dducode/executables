using System.Runtime.CompilerServices;

namespace Interactions.Validation;

internal sealed class MoreThanValidator<T>(T comparand, IComparer<T> comparer) : Validator<T> {

  public override string ErrorMessage { get; } = $"Value must be more than {comparand}";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool IsValid(T value) {
    return comparer.Compare(value, comparand) > 0;
  }

}