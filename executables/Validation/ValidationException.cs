namespace Executables.Validation;

public abstract class ValidationException(string message) : Exception(message);

public class InvalidInputException : ValidationException {

  internal InvalidInputException(string message) : base(message) {
  }

}

public class InvalidOutputException : ValidationException {

  internal InvalidOutputException(string message) : base(message) {
  }

}