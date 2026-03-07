using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Branches;

public sealed class AsyncBranchBuilder<T1, T2> {

  private readonly List<(Func<bool> condition, IAsyncExecutable<T1, T2> executable)> _nodes = [];

  private AsyncBranchBuilder(Func<bool> condition, IAsyncExecutable<T1, T2> executable) {
    _nodes.Add((condition, executable));
  }

  internal static AsyncBranchBuilder<T1, T2> If(Func<bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncBranchBuilder<T1, T2>(condition, executable);
  }

  public AsyncBranchBuilder<T1, T2> ElseIf(Func<bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    _nodes.Add((condition, executable));
    return this;
  }

  [Pure]
  public IAsyncExecutable<T1, T2> Else(IAsyncExecutable<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(handler, (current, node) => new AsyncConditionalExecutable<T1, T2>(node.condition, node.executable, current));
  }

}