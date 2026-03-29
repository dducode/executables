using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables.Branches;

/// <summary>
/// Builds an asynchronous conditional branch pipeline.
/// </summary>
public readonly struct AsyncBranchBuilder<T1, T2> {

  private readonly List<(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable)> _nodes = [];

  private AsyncBranchBuilder(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    _nodes.Add((condition, executable));
  }

  internal static AsyncBranchBuilder<T1, T2> If(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncBranchBuilder<T1, T2>(condition, executable);
  }

  /// <summary>
  /// Adds another conditional branch checked after previously added branches.
  /// </summary>
  /// <param name="condition">Condition that selects this branch.</param>
  /// <param name="executable">Executable invoked when <paramref name="condition"/> returns <see langword="true"/>.</param>
  /// <returns>Updated branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public AsyncBranchBuilder<T1, T2> ElseIf(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    _nodes.Add((condition, executable));
    return this;
  }

  /// <summary>
  /// Finalizes the branch chain with a fallback executable.
  /// </summary>
  /// <param name="executable">Executable used when no branch condition matches.</param>
  /// <returns>Composed asynchronous executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executable"/> is <see langword="null"/>.</exception>
  [Pure]
  public IAsyncExecutable<T1, T2> Else(IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return _nodes
      .AsEnumerable()
      .Reverse()
      .Aggregate(executable, (current, node) => new AsyncConditionalExecutable<T1, T2>(node.condition, node.executable, current));
  }

}