using System.Diagnostics.Contracts;
using Interactions.Core.Executables;
using Interactions.Internal;

namespace Interactions.Branches;

/// <summary>
/// Builds conditional branch pipeline.
/// </summary>
public readonly struct BranchBuilder<T1, T2> {

  private readonly List<(Func<T1, bool> condition, IExecutable<T1, T2> executable)> _nodes = [];

  private BranchBuilder(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    _nodes.Add((condition, executable));
  }

  internal static BranchBuilder<T1, T2> If(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new BranchBuilder<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Adds another conditional branch checked after previously added branches.
  /// </summary>
  /// <param name="condition">Condition that selects this branch.</param>
  /// <param name="executable">Executable invoked when <paramref name="condition"/> returns <see langword="true"/>.</param>
  /// <returns>Updated branch builder.</returns>
  public BranchBuilder<T1, T2> ElseIf(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    _nodes.Add((condition, executable));
    return this;
  }

  /// <summary>
  /// Finalizes the branch chain with a fallback executable.
  /// </summary>
  /// <param name="executable">Executable used when no branch condition matches.</param>
  /// <returns>Composed executable with all branch conditions.</returns>
  [Pure]
  public IExecutable<T1, T2> Else(IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(executable, (current, node) => new ConditionalExecutable<T1, T2>(node.condition, node.executable, current));
  }

}