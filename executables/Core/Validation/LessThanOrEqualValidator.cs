using Executables.Validation;

namespace Executables.Core.Validation;

internal sealed class LessThanOrEqualValidator<T>(T comparand, IComparer<T> comparer) : Validator<T> {

  public override string ErrorMessage { get; } = $"Value must be less than {comparand}";

  public override bool IsValid(T value) {
    return comparer.Compare(value, comparand) <= 0;
  }

}