namespace Interactions.Guards;

/// <summary>
/// Exception thrown when guard denies access.
/// </summary>
public class AccessDeniedException : Exception {

  internal AccessDeniedException(string message) : base(message) { }

}
