namespace Executables.Policies;

/// <summary>
/// Represents an error when reentrant policy execution is detected.
/// </summary>
public sealed class ReentranceException : InvalidOperationException {

  internal ReentranceException(string message) : base(message) { }

}