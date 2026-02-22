using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public class AsyncBranchBuilder<T1, T2> {

  private readonly List<(Func<bool> condition, AsyncHandler<T1, T2> handler)> _nodes = [];

  private AsyncBranchBuilder(Func<bool> condition, AsyncHandler<T1, T2> handler) {
    _nodes.Add((condition, handler));
  }

  internal static AsyncBranchBuilder<T1, T2> If(Func<bool> condition, AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return new AsyncBranchBuilder<T1, T2>(condition, handler);
  }

  public AsyncBranchBuilder<T1, T2> ElseIf(Func<bool> condition, AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    _nodes.Add((condition, handler));
    return this;
  }

  [Pure]
  public AsyncHandler<T1, T2> Else(AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(handler, (current, node) => new AsyncConditionalHandler<T1, T2>(node.condition, node.handler, current));
  }

}