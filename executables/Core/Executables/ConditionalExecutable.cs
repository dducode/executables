namespace Executables.Core.Executables;

internal sealed class ConditionalExecutable<T1, T2>(
  Func<T1, bool> condition,
  IExecutable<T1, T2> ifExecutable,
  IExecutable<T1, T2> elseExecutable) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  private readonly IExecutor<T1, T2> _ifExecutor = ifExecutable.GetExecutor();
  private readonly IExecutor<T1, T2> _elseExecutor = elseExecutable.GetExecutor();

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return condition(input) ? _ifExecutor.Execute(input) : _elseExecutor.Execute(input);
  }

}