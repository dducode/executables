namespace Interactions.Guards;

/// <summary>
/// Guard with runtime-switchable state.
/// Opened state denies access; closed state grants access.
/// </summary>
/// <param name="errorMessage">Message returned when access is denied.</param>
public class ManualGuard(string errorMessage) : Guard {

  public override string ErrorMessage => errorMessage;

  private int _opened;

  /// <summary>
  /// Opens the guard and blocks subsequent invocations.
  /// </summary>
  public void Open() {
    Interlocked.Exchange(ref _opened, 1);
  }

  /// <summary>
  /// Closes the guard and allows subsequent invocations to pass.
  /// </summary>
  public void Close() {
    Interlocked.Exchange(ref _opened, 0);
  }

  public override bool TryGetAccess() {
    return Volatile.Read(ref _opened) == 0;
  }

}