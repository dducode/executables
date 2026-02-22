using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public class BranchBuilder<T1, T2> {

  private readonly List<(Func<bool> condition, Handler<T1, T2> handler)> _nodes = [];

  private BranchBuilder(Func<bool> condition, Handler<T1, T2> handler) {
    _nodes.Add((condition, handler));
  }

  internal static BranchBuilder<T1, T2> If(Func<bool> condition, Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return new BranchBuilder<T1, T2>(condition, handler);
  }

  public BranchBuilder<T1, T2> ElseIf(Func<bool> condition, Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    _nodes.Add((condition, handler));
    return this;
  }

  [Pure]
  public Handler<T1, T2> Else(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(handler, (current, node) => new ConditionalHandler<T1, T2>(node.condition, node.handler, current));
  }

}