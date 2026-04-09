namespace Executables.Guards;

/// <summary>
/// Guard with runtime-switchable access state.
/// By default, access is allowed until <see cref="Deny"/> is called.
/// </summary>
/// <param name="errorMessage">Message returned when access is denied.</param>
public class ToggleGuard(string errorMessage) : Guard {

  public override string ErrorMessage => errorMessage;

  private int _denied;

  /// <summary>
  /// Denies access for subsequent invocations.
  /// </summary>
  public void Deny() {
    Interlocked.Exchange(ref _denied, 1);
  }

  /// <summary>
  /// Allows access for subsequent invocations.
  /// </summary>
  public void Allow() {
    Interlocked.Exchange(ref _denied, 0);
  }

  public override bool TryGetAccess() {
    return Volatile.Read(ref _denied) == 0;
  }

}