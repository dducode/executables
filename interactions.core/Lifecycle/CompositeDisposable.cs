using System.Runtime.ExceptionServices;

namespace Interactions.Core.Lifecycle;

internal sealed class CompositeDisposable(IDisposable first, IDisposable second) : IDisposable {

  private int _disposed;

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;

    Exception firstExc = null;
    Exception secondExc = null;

    try {
      first.Dispose();
    }
    catch (Exception e) {
      firstExc = e;
    }

    try {
      second.Dispose();
    }
    catch (Exception e) {
      secondExc = e;
    }

    if (firstExc != null && secondExc != null)
      throw new AggregateException(firstExc, secondExc);
    if (firstExc != null)
      ExceptionDispatchInfo.Capture(firstExc).Throw();
    if (secondExc != null)
      ExceptionDispatchInfo.Capture(secondExc).Throw();
  }

}