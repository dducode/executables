using Interactions.Core;

namespace Interactions.Handlers;

internal sealed class AsyncAnonymousDisposeHandler<T1, T2>(AsyncHandler<T1, T2> inner, Action dispose) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return inner.Execute(input, token);
  }

  protected override void DisposeCore() {
    try {
      dispose();
    }
    finally {
      inner.Dispose();
    }
  }

}