namespace Interactions.Core.Internal;

internal sealed class Lazy<T>(IResolver<T> resolver) where T : class {

  public T Value {
    get {
      T value = Volatile.Read(ref _value);

      if (value != null)
        return value;

      lock (_lock) {
        value = _value;
        if (value != null)
          return value;

        return _value = resolver.Resolve() ?? throw new InvalidOperationException($"Cannot resolve {typeof(T).Name} by {resolver.GetType().Name}");
      }
    }
  }

  public T ValueOrDefault => Volatile.Read(ref _value);

  private readonly object _lock = new();
  private T _value;

}