using Interactions.Core;

namespace Interactions.Executables;

internal sealed class ContinuationExecutable<T1, T2>(IExecutable<T1, IExecutable<T1, T2>> continuation) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    IExecutable<T1, T2> inner = continuation.Execute(input);
    return inner != null ? inner.Execute(input) : throw new InvalidOperationException($"Cannot get executable from {continuation.GetType().Name}");
  }

}