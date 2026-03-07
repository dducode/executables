namespace Interactions.Core.Resolvers;

internal sealed class AnonymousResolver<T>(Func<T> resolveFunc) : IResolver<T> {

  public T Resolve() {
    return resolveFunc();
  }

}
