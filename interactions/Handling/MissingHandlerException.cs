namespace Interactions.Handling;

public sealed class MissingHandlerException(string message) : InvalidOperationException(message);