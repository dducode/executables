namespace Executables.Handling;

/// <summary>
/// Exception thrown when no handler is available for the requested operation.
/// </summary>
public sealed class MissingHandlerException(string message) : InvalidOperationException(message);