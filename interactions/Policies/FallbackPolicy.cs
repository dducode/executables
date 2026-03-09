using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

internal sealed class FallbackPolicy<T1, T2, TEx>(IFallbackHandler<T1, TEx, T2> handler) : Policy<T1, T2> where TEx : Exception {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    try {
      return executable.Execute(input);
    }
    catch (TEx e) {
      return handler.Fallback(input, e);
    }
  }

}