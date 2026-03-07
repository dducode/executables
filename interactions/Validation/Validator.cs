namespace Interactions.Validation;

public abstract class Validator<T> {

  public abstract string ErrorMessage { get; }

  public abstract bool IsValid(T value);

}