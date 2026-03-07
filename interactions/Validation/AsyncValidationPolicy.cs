using Interactions.Core;
using Interactions.Policies;

namespace Interactions.Validation;

internal sealed class AsyncValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = await executable.Execute(input, token);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}