namespace Interactions.Core;

public sealed class MissingHandlerException(string message) : InvalidOperationException(message);