using Interactions.Core;

namespace Interactions.Guards;

internal sealed class DynamicGuard(IProvider<Guard> provider) : Guard {

  public override string ErrorMessage => _errorMessage;
  private string _errorMessage;

  public override bool TryGetAccess() {
    Guard guard = provider.Get();
    _errorMessage = guard?.ErrorMessage;
    return guard?.TryGetAccess() ?? throw new InvalidOperationException($"Cannot resolve guard by {provider.GetType().Name}");
  }

}