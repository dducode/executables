using System.Runtime.CompilerServices;

namespace Executables.Context;

/// <summary>
/// Represents a read-only executable context.
/// </summary>
public interface IReadonlyContext : IFormattable {

  /// <summary>
  /// Parent context, or <see langword="null"/> for the root context.
  /// </summary>
  IReadonlyContext Parent { get; }

  /// <summary>
  /// Context name.
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Correlation identifier shared by related contexts.
  /// </summary>
  Guid CorrelationId { get; }

  /// <summary>
  /// Unique context identifier.
  /// </summary>
  long ContextId { get; }

  /// <summary>
  /// Nesting depth relative to the root context.
  /// </summary>
  int Depth { get; }

  /// <summary>
  /// Root context of the current hierarchy.
  /// </summary>
  IReadonlyContext Root { get; }

  /// <summary>
  /// Tries to get a value from the current context or its ancestors.
  /// </summary>
  bool TryGet<T>(object key, out T value);

  /// <summary>
  /// Tries to get a value from the current context only.
  /// </summary>
  bool TryGetLocal<T>(object key, out T value);

}

/// <summary>
/// Provides access to the current executable context through <see cref="Current"/>.
/// </summary>
public sealed class ExecutableContext : IDisposable, IReadonlyContext {

  /// <summary>
  /// Current ambient executable context.
  /// </summary>
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

  internal ExecutableContext(IReadonlyContext parent) {
    Root = parent?.Root ?? this;
    Parent = parent;
    CorrelationId = parent?.CorrelationId ?? Guid.NewGuid();
    ContextId = Interlocked.Increment(ref _id);
    Depth = parent?.Depth + 1 ?? 0;
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

  internal void Set<T>(object key, T value) {
    ThrowIfDisposed();
    _context[key] = value;
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

  void IDisposable.Dispose() {
    if (_disposed)
      return;

    _disposed = true;
    _context.Clear();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void ThrowIfDisposed() {
    if (_disposed)
      throw new ObjectDisposedException(nameof(ExecutableContext));
  }

}