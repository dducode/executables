using Interactions.Core;

namespace Interactions.Validation;

internal sealed class ValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : Policy<T1, T2> {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = executable.Execute(input);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}