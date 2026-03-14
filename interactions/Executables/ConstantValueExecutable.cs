using Interactions.Core;

namespace Interactions.Executables;

internal sealed class ConstantValueExecutable<T1, T2>(T2 value) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  public IExecutor<T1, T2> GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return value;
  }

}