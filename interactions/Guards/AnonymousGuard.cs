namespace Interactions.Guards;

internal sealed class AnonymousGuard(Func<bool> condition, string errorMessage) : Guard {

  public override string ErrorMessage { get; } = errorMessage;

  public override bool TryGetAccess() {
    return condition();
  }

}
