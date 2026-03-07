using System.Runtime.CompilerServices;

namespace Interactions.Context;

public interface IReadonlyContext : IFormattable {

  IReadonlyContext Parent { get; }
  string Name { get; }
  Guid Id { get; }

  bool TryGet<T>(object key, out T value);
  bool TryGetLocal<T>(object key, out T value);

}

public sealed class InteractionContext : IDisposable, IReadonlyContext {

  public static IReadonlyContext Current {
    get => _current.Value;
    internal set => _current.Value = value;
  }

  private static readonly AsyncLocal<IReadonlyContext> _current = new();

  public IReadonlyContext Parent { get; }
  public string Name { get; set; }
  public Guid Id { get; }

  private readonly Dictionary<object, object> _context = new();
  private bool _disposed;

  internal InteractionContext(IReadonlyContext parent, Guid id) {
    Parent = parent;
    Id = id;
  }

  public void Set<T>(object key, T value) {
    ThrowIfDisposed();
    _context[key] = value;
  }

  public bool TryGet<T>(object key, out T value) {
    ThrowIfDisposed();

    if (TryGetLocal(key, out value))
      return true;

    return Parent?.TryGet(key, out value) ?? false;
  }

  public bool TryGetLocal<T>(object key, out T value) {
    ThrowIfDisposed();

    if (_context.TryGetValue(key, out object obj) && obj is T result) {
      value = result;
      return true;
    }

    value = default;
    return false;
  }

  public override string ToString() {
    return $"Name: {Name ?? "Null"}, Id: {Id}";
  }

  public string ToString(string format, IFormatProvider formatProvider) {
    return string.Equals(format, "v", StringComparison.OrdinalIgnoreCase) ? ToStringVerbose() : ToString();
  }

  private string ToStringVerbose() {
    return Parent == null ? $"{ToString()}, Parent: Null" : $"{ToString()}, Parent: {{\n\t{Parent:v}\n}}";
  }

  public void Dispose() {
    if (_disposed)
      return;

    _disposed = true;
    _context.Clear();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void ThrowIfDisposed() {
    if (_disposed)
      throw new ObjectDisposedException(nameof(InteractionContext));
  }

}