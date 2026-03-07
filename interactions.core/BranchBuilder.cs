using System.Diagnostics.Contracts;

namespace Interactions.Core;

public class BranchBuilder<T1, T2> {

  private readonly List<(Func<bool> condition, IExecutable<T1, T2> executable)> _nodes = [];

  private BranchBuilder(Func<bool> condition, IExecutable<T1, T2> executable) {
    _nodes.Add((condition, executable));
  }

  internal static BranchBuilder<T1, T2> If(Func<bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new BranchBuilder<T1, T2>(condition, executable);
  }

  public BranchBuilder<T1, T2> ElseIf(Func<bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    _nodes.Add((condition, executable));
    return this;
  }

  [Pure]
  public IExecutable<T1, T2> Else(IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(executable, (current, node) => new ConditionalExecutable<T1, T2>(node.condition, node.executable, current));
  }

}