using Interactions.Fallbacks;
using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class FallbackPolicy<T1, T2, TEx>(IFallbackHandler<T1, TEx, T2> handler) : Policy<T1, T2> where TEx : Exception {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return executor.Execute(input);
    }
    catch (TEx e) {
      return handler.Fallback(input, e);
    }
  }

}