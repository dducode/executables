namespace Executables.Operations;

internal sealed class ThreadPoolOperator<T> : BehaviorOperator<T, Unit> {

  public override Unit Invoke(T input, IExecutor<T, Unit> executor) {
    ThreadPool.QueueUserWorkItem(_ => executor.Execute(input));
    return default;
  }

}