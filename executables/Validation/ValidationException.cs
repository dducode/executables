namespace Executables.Validation;

/// <summary>
/// Base exception type for validation failures.
/// </summary>
public abstract class ValidationException(string message) : Exception(message);

/// <summary>
/// Exception thrown when input validation fails.
/// </summary>
public class InvalidInputException : ValidationException {

  internal InvalidInputException(string message) : base(message) { }

}

/// <summary>
/// Exception thrown when output validation fails.
/// </summary>
public class InvalidOutputException : ValidationException {

  internal InvalidOutputException(string message) : base(message) { }

}