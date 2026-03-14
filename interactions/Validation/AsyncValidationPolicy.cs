using Interactions.Core;

namespace Interactions.Validation;

internal sealed class AsyncValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = await executor.Execute(input, token);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}