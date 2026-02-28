using Interactions.Core;
using Interactions.Policies;

namespace Interactions.Validation;

internal sealed class ValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : Policy<T1, T2> {

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = invocation.Invoke(input);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}

internal sealed class AsyncValidationPolicy<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);

    T2 output = await invocation.Invoke(input, token);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

}