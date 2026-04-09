namespace Executables.Events;

/// <summary>
/// Specifies the order in which subscribers are notified during sequential publishing.
/// </summary>
public enum PublishOrder {

  /// <summary>
  /// Notifies subscribers in registration order.
  /// </summary>
  Direct,
  /// <summary>
  /// Notifies subscribers in reverse registration order.
  /// </summary>
  Reverse

}