namespace Interactions;

/// <summary>
/// Represents the absence of a meaningful value.
/// </summary>
public struct Unit : IEquatable<Unit> {

  public static bool operator ==(Unit left, Unit right) {
    return true;
  }

  public static bool operator !=(Unit left, Unit right) {
    return false;
  }

  public bool Equals(Unit other) {
    return true;
  }

  public override bool Equals(object obj) {
    return obj is Unit;
  }

  public override int GetHashCode() {
    return 0;
  }

  public override string ToString() {
    return string.Empty;
  }

}