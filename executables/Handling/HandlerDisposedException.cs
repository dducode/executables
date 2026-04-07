namespace Executables.Handling;

/// <summary>
/// Exception thrown when an operation is attempted on a disposed handler.
/// </summary>
public sealed class HandlerDisposedException : ObjectDisposedException {

  internal HandlerDisposedException(string objectName) : base(objectName) { }

}