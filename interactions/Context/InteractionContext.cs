using System.Runtime.CompilerServices;

namespace Interactions.Context;

public interface IReadonlyContext : IFormattable {

  IReadonlyContext Parent { get; }
  string Name { get; }
  Guid CorrelationId { get; }
  long ContextId { get; }
  int Depth { get; }
  IReadonlyContext Root { get; }

  bool TryGet<T>(object key, out T value);
  bool TryGetLocal<T>(object key, out T value);

}

public sealed class InteractionContext : IDisposable, IReadonlyContext {

  public static IReadonlyContext Current {
    get => _current.Value;
    internal set => _current.Value = value;
  }

  private static long _id;
  private static readonly AsyncLocal<IReadonlyContext> _current = new();

  public IReadonlyContext Root { get; }
  public IReadonlyContext Parent { get; }
  public Guid CorrelationId { get; }
  public long ContextId { get; }
  public int Depth { get; }
  public string Name { get; set; }

  private readonly Dictionary<object, object> _context = new();
  private bool _disposed;

  internal InteractionContext(IReadonlyContext parent) {
    Root = parent?.Root ?? this;
    Parent = parent;
    CorrelationId = parent?.CorrelationId ?? Guid.NewGuid();
    ContextId = Interlocked.Increment(ref _id);
    Depth = parent?.Depth + 1 ?? 0;
  }

  public void Set<T>(object key, T value) {
    ThrowIfDisposed();
    _context[key] = value;
  }

  public bool TryGet<T>(object key, out T value) {
    ThrowIfDisposed();

    for (IReadonlyContext context = this; context != null; context = context.Parent)
      if (context.TryGetLocal(key, out value))
        return true;

    value = default;
    return false;
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
    return $"[{CorrelationId}] {Name ?? "Context"}({ContextId}:{Depth})";
  }

  public string ToString(string format, IFormatProvider formatProvider) {
    return string.Equals(format, "v", StringComparison.OrdinalIgnoreCase) ? ToStringVerbose() : ToString();
  }

  private string ToStringVerbose() {
    return Parent == null ? ToString() : $"{Parent:v}{Environment.NewLine}{ToString()}";
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