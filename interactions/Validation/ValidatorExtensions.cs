using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Validation;

public static class ValidatorExtensions {

  [Pure]
  public static Validator<T> And<T>(this Validator<T> first, Validator<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AndValidator<T>(first, second);
  }

  [Pure]
  public static Validator<T> Or<T>(this Validator<T> first, Validator<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new OrValidator<T>(first, second);
  }

  [Pure]
  public static Validator<T> OverrideMessage<T>(this Validator<T> validator, string message) {
    ExceptionsHelper.ThrowIfNullOrEmpty(message, nameof(message));
    return new OverrideMessageValidator<T>(validator, message);
  }

}