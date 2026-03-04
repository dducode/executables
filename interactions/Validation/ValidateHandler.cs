using Interactions.Core;

namespace Interactions.Validation;

[Obsolete]
internal sealed class ValidateHandler<T1, T2>(
  Validator<T1> inputValidator,
  Handler<T1, T2> inner,
  Validator<T2> outputValidator) : Handler<T1, T2> {

  protected override T2 HandleCore(T1 input) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);
    T2 output = inner.Handle(input);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}

[Obsolete]
internal sealed class AsyncValidateHandler<T1, T2>(
  Validator<T1> inputValidator,
  AsyncHandler<T1, T2> inner,
  Validator<T2> outputValidator) : AsyncHandler<T1, T2> {

  protected override async ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    if (!inputValidator.IsValid(input))
      throw new InvalidInputException(inputValidator.ErrorMessage);
    T2 output = await inner.Handle(input, token);

    if (!outputValidator.IsValid(output))
      throw new InvalidOutputException(outputValidator.ErrorMessage);
    return output;
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}