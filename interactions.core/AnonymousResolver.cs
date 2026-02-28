namespace Interactions.Core;

internal sealed class AnonymousResolver<T>(Func<T> resolveFunc) : IResolver<T> {

  public T Resolve() {
    return resolveFunc();
  }

}
