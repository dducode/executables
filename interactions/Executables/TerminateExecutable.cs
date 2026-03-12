using Interactions.Core;

namespace Interactions.Executables;

internal sealed class TerminateExecutable<T1, T2>(IExecutable<T1, T2> first, IExecutable<T2> second) : IExecutable<T1> {

  public void Execute(T1 input) {
    second.Execute(first.Execute(input));
  }

}