using Interactions.Core.Handlers;
using R3;
using Unit = Interactions.Core.Unit;

namespace Interactions.R3;

internal sealed class ObserverProxy<T>(Handler<T, Unit> inner) : Observer<T> {

  protected override void OnNextCore(T value) {
    inner.Execute(value);
  }

  protected override void OnErrorResumeCore(Exception error) {
    throw error;
  }

  protected override void OnCompletedCore(Result result) {
    if (result.Exception != null)
      throw result.Exception;
  }

}