using Executables.Guards;

namespace Executables.Core.Guards;

internal sealed class CompositeGuard(Guard first, Guard second) : Guard {

  public override string ErrorMessage => $"{first.ErrorMessage} or {second.ErrorMessage}";

  public override bool TryGetAccess() {
    return first.TryGetAccess() && second.TryGetAccess();
  }

}
