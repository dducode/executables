using Interactions.Core;
using Interactions.Policies;
using Interactions.Transformation;
using Interactions.Validation;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static T2 Execute<T1, T2>(this IExecutable<T1, T2> executable, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, executable, init);
  }

  public static T2 Execute<T1, T2>(this Policy<T1, T2> policy, T1 input, IExecutable<T1, T2> executable, Action<InteractionContext> init) {
    return InvokeWithContext(input, Executable.Create((T1 i) => policy.Execute(i, executable)), init);
  }

  public static bool IsValid<T>(this Validator<T> validator, T input, Action<InteractionContext> init) {
    return InvokeWithContext(input, Executable.Create<T, bool>(validator.IsValid), init);
  }

  public static T2 Transform<T1, T2>(this Transformer<T1, T2> transformer, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, Executable.Create<T1, T2>(transformer.Transform), init);
  }

  private static T2 InvokeWithContext<T1, T2>(T1 input, IExecutable<T1, T2> executable, Action<InteractionContext> init) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return executable.Execute(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

}