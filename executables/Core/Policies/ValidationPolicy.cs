using Executables.Policies;
using Executables.Validation;

namespace Executables.Core.Policies;

internal sealed class ValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = executor.Execute(input);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}