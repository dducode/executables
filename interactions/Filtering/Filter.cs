using Interactions.Core;

namespace Interactions.Filtering;

public interface IFilter<T> : IExecutable<IEnumerable<T>, IEnumerable<T>>;

internal sealed class Filter<T>(IExecutable<IEnumerable<T>, IEnumerable<T>> executable) : IFilter<T> {

  public IEnumerable<T> Execute(IEnumerable<T> input) {
    return executable.Execute(input);
  }

}