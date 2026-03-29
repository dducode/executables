namespace Executables.Context;

public readonly ref struct ContextWriter {

  private readonly ExecutableContext _context;

  internal ContextWriter(ExecutableContext context) {
    _context = context;
  }

  public string Name {
    get => _context.Name;
    set => _context.Name = value;
  }

  public void Set<T>(object key, T value) {
    _context.Set(key, value);
  }

}