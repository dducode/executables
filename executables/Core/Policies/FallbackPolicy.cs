using Executables.Policies;

namespace Executables.Core.Policies;

internal sealed class FallbackPolicy<T1, T2, TEx>(Func<T1, TEx, T2> fallback) : Policy<T1, T2> where TEx : Exception {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return executor.Execute(input);
    }
    catch (TEx e) {
      return fallback(input, e);
    }
  }

}