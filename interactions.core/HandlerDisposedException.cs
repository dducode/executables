namespace Interactions.Core;

public sealed class HandlerDisposedException : ObjectDisposedException {

  internal HandlerDisposedException(string objectName) : base(objectName) { }

}