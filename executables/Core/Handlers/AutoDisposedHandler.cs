using Executables.Handling;

namespace Executables.Core.Handlers;

internal sealed class AutoDisposedHandler<T1, T2>(Handler<T1, T2> inner) : Handler<T1, T2> {

  protected override T2 HandleCore(T1 input) {
    try {
      return inner.Handle(input);
    }
    catch (Exception) {
      Dispose();
      throw;
    }
  }

}