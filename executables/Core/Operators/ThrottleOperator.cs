using System.Diagnostics;
using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ThrottleOperator<T>(TimeSpan interval) : BehaviorOperator<T, Unit> {

  private readonly Dictionary<IExecutor<T, Unit>, Stopwatch> _stopwatches = new();
  private readonly object _lock = new();

  public override Unit Invoke(T input, IExecutor<T, Unit> executor) {
    lock (_lock) {
      if (!_stopwatches.TryGetValue(executor, out Stopwatch stopwatch)) {
        stopwatch = new Stopwatch();
        _stopwatches.Add(executor, stopwatch);
      }

      if (stopwatch.IsRunning && stopwatch.Elapsed < interval)
        return default;

      stopwatch.Restart();
    }

    executor.Execute(input);
    return default;
  }

}