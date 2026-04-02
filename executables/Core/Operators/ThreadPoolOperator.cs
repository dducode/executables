using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ThreadPoolOperator<T> : BehaviorOperator<T, Unit> {

  internal static ThreadPoolOperator<T> Instance { get; } = new();

  private ThreadPoolOperator() { }

  public override Unit Invoke(T input, IExecutor<T, Unit> executor) {
    ThreadPool.QueueUserWorkItem(_ => executor.Execute(input));
    return default;
  }

}