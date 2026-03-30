using System.Diagnostics.Contracts;
using Executables.Core.Analytics;
using Executables.Internal;

namespace Executables.Analytics;

/// <summary>
/// Factory methods for creating metrics sinks.
/// </summary>
public static class Metrics {

  /// <summary>
  /// Creates a metrics sink from delegates.
  /// </summary>
  /// <param name="call">Callback invoked when execution starts.</param>
  /// <param name="success">Callback invoked when execution succeeds.</param>
  /// <param name="failure">Optional callback invoked when execution fails.</param>
  /// <param name="latency">Optional callback invoked with measured execution latency.</param>
  /// <returns>Metrics sink backed by the provided delegates.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="call"/> or <paramref name="success"/> is <see langword="null"/>.
  /// </exception>
  [Pure]
  public static IMetrics<T1, T2> Create<T1, T2>(Action<T1> call, Action<T2> success, Action<Exception> failure = null, Action<TimeSpan> latency = null) {
    ExceptionsHelper.ThrowIfNull(call, nameof(call));
    ExceptionsHelper.ThrowIfNull(success, nameof(success));
    return new AnonymousMetrics<T1, T2>(call, success, failure, latency);
  }

}