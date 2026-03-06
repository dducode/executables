using Interactions.Core;

namespace Interactions.Guards;

internal sealed class LazyGuard(IResolver<Guard> resolver) : Guard {

  public override string ErrorMessage => _guard.Value.ErrorMessage;

  private readonly Core.Lazy<Guard> _guard = new(resolver);

  public override bool TryGetAccess() {
    return _guard.Value.TryGetAccess();
  }

}