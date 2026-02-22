using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions;

public abstract class Decorator<T1, T2> {

  public abstract T2 Decorate(T1 item);

}

public static class Decorator {

  [Pure]
  public static Decorator<T1, T2> FromMethod<T1, T2>(Func<T1, T2> decoration) {
    ExceptionsHelper.ThrowIfNull(decoration, nameof(decoration));
    return new AnonymousDecorator<T1, T2>(decoration);
  }

}