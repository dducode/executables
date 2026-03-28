using Interactions.Handling;

namespace Interactions.Core.Handlers;

internal sealed class AsyncAutoDisposedHandler<T1, T2>(AsyncHandler<T1, T2> inner) : AsyncHandler<T1, T2> {

  protected override async ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    try {
      return await inner.Handle(input, token);
    }
    catch (Exception) {
      Dispose();
      throw;
    }
  }

}