namespace Executables.Core.Executors;

internal sealed class ThreeArgsPartialExecutor<T1, T2, T3, T4, T5>(IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2, T3 t3) : IExecutor<T4, T5> {

  T5 IExecutor<T4, T5>.Execute(T4 input) {
    return executor.Execute(t1, t2, t3, input);
  }

}

internal sealed class TwoArgsPartialExecutor<T1, T2, T3, T4, T5>(IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2) : IExecutor<(T3, T4), T5> {

  T5 IExecutor<(T3, T4), T5>.Execute((T3, T4) input) {
    return executor.Execute(t1, t2, input.Item1, input.Item2);
  }

}

internal sealed class OneArgPartialExecutor<T1, T2, T3, T4, T5>(IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1) : IExecutor<(T2, T3, T4), T5> {

  T5 IExecutor<(T2, T3, T4), T5>.Execute((T2, T3, T4) input) {
    return executor.Execute(t1, input.Item1, input.Item2, input.Item3);
  }

}

internal sealed class TwoArgsPartialExecutor<T1, T2, T3, T4>(IExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2) : IExecutor<T3, T4> {

  T4 IExecutor<T3, T4>.Execute(T3 input) {
    return executor.Execute(t1, t2, input);
  }

}

internal sealed class OneArgPartialExecutor<T1, T2, T3, T4>(IExecutor<(T1, T2, T3), T4> executor, T1 t1) : IExecutor<(T2, T3), T4> {

  T4 IExecutor<(T2, T3), T4>.Execute((T2, T3) input) {
    return executor.Execute(t1, input.Item1, input.Item2);
  }

}

internal sealed class PartialExecutor<T1, T2, T3>(IExecutor<(T1, T2), T3> executor, T1 t1) : IExecutor<T2, T3> {

  T3 IExecutor<T2, T3>.Execute(T2 input) {
    return executor.Execute(t1, input);
  }

}