namespace Interactions.Context;

public readonly ref struct ContextWriter {

  private readonly InteractionContext _context;

  internal ContextWriter(InteractionContext context) {
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