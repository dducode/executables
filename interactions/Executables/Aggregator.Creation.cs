using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static class Aggregator {

  [Pure]
  public static IAggregator<T1, T2, T3> Create<T1, T2, T3>(Func<T1, T2, T3> aggregation) {
    ExceptionsHelper.ThrowIfNull(aggregation, nameof(aggregation));
    return new AnonymousAggregator<T1, T2, T3>(aggregation);
  }

}