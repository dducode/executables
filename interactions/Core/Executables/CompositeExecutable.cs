using System.Runtime.CompilerServices;

namespace Interactions.Core.Executables;

internal sealed class CompositeExecutable<T1, T2, T3>(IExecutable<T1, T2> first, IExecutable<T2, T3> second) : IExecutable<T1, T3>, IExecutor<T1, T3> {

  private readonly IExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IExecutor<T2, T3> _second = second.GetExecutor();

  IExecutor<T1, T3> IExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  T3 IExecutor<T1, T3>.Execute(T1 input) {
    return _second.Execute(_first.Execute(input));
  }

}