namespace Executables.Handling;

public sealed class HandlerDisposedException : ObjectDisposedException {

  internal HandlerDisposedException(string objectName) : base(objectName) { }

}