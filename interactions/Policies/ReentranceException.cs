namespace Interactions.Policies;

public sealed class ReentranceException : InvalidOperationException {

  internal ReentranceException(string message) : base(message) { }

}