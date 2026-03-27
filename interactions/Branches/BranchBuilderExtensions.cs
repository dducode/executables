using System.Diagnostics.Contracts;
using Interactions.Internal;

namespace Interactions.Branches;

public static partial class BranchBuilderExtensions {

  public static BranchBuilder<T1, T2> ElseIf<T1, T2>(this BranchBuilder<T1, T2> builder, Func<T1, bool> condition, Func<T1, T2> action) {
    return builder.ElseIf(condition, Executable.Create(action));
  }

  public static BranchBuilder<Unit, T> ElseIf<T>(this BranchBuilder<Unit, T> builder, Func<bool> condition, Func<T> action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), Executable.Create(action));
  }

  public static BranchBuilder<T, Unit> ElseIf<T>(this BranchBuilder<T, Unit> builder, Func<T, bool> condition, Action<T> action) {
    return builder.ElseIf(condition, Executable.Create(action));
  }

  public static BranchBuilder<Unit, Unit> ElseIf(this BranchBuilder<Unit, Unit> builder, Func<bool> condition, Action action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), Executable.Create(action));
  }

  [Pure]
  public static IExecutable<T1, T2> Else<T1, T2>(this BranchBuilder<T1, T2> builder, Func<T1, T2> action) {
    return builder.Else(Executable.Create(action));
  }

  [Pure]
  public static IExecutable<Unit, T> Else<T>(this BranchBuilder<Unit, T> builder, Func<T> action) {
    return builder.Else(Executable.Create(action));
  }

  [Pure]
  public static IExecutable<T, Unit> Else<T>(this BranchBuilder<T, Unit> builder, Action<T> action) {
    return builder.Else(Executable.Create(action));
  }

  [Pure]
  public static IExecutable<Unit, Unit> Else(this BranchBuilder<Unit, Unit> builder, Action action) {
    return builder.Else(Executable.Create(action));
  }

}