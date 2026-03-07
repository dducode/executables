using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Decorators;

public static class Decorator {

  [Pure]
  public static Decorator<T1, T2> Create<T1, T2>(Func<T1, T2> decoration) {
    ExceptionsHelper.ThrowIfNull(decoration, nameof(decoration));
    return new AnonymousDecorator<T1, T2>(decoration);
  }

}