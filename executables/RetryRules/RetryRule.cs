namespace Executables.RetryRules;

/// <summary>
/// Defines retry decision logic for a specific exception type.
/// </summary>
/// <typeparam name="TException">Exception type handled by this rule.</typeparam>
public interface IRetryRule<in TException> where TException : Exception {

  /// <summary>
  /// Decides whether failed invocation should be retried.
  /// </summary>
  /// <param name="attemptsCount">Number of failed attempts so far.</param>
  /// <param name="exception">Exception that caused current failure.</param>
  /// <param name="token">Cancellation token for asynchronous rule work.</param>
  /// <returns><see langword="true"/> to retry; otherwise <see langword="false"/>.</returns>
  ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token);

}