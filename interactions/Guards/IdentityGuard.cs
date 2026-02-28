namespace Interactions.Guards;

internal sealed class IdentityGuard : Guard {

  internal static IdentityGuard Instance { get; } = new();

  private IdentityGuard() { }

  public override string ErrorMessage => string.Empty;

  public override bool TryGetAccess() {
    return true;
  }

}