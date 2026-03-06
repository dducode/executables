using Interactions.Core;

namespace Interactions.Policies;

internal sealed class ThreadPoolPolicy<T> : Policy<T, Unit> {

  public override Unit Execute(T input, Func<T, Unit> invocation) {
    ThreadPool.QueueUserWorkItem(_ => invocation.Invoke(input));
    return default;
  }

}