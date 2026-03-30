namespace Executables.Context;

/// <summary>
/// Provides write access to an executable context during initialization.
/// </summary>
public readonly ref struct ContextWriter {

  private readonly ExecutableContext _context;

  internal ContextWriter(ExecutableContext context) {
    _context = context;
  }

  /// <summary>
  /// Gets or sets the context name.
  /// </summary>
  public string Name {
    get => _context.Name;
    set => _context.Name = value;
  }

  /// <summary>
  /// Stores a value in the context under the specified key.
  /// </summary>
  /// <param name="key">Context key.</param>
  /// <param name="value">Value to store.</param>
  public void Set<T>(object key, T value) {
    _context.Set(key, value);
  }

}