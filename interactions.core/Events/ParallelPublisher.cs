using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using Interactions.Core.Internal;

namespace Interactions.Core.Events;

internal sealed class ParallelPublisher<T>(ParallelOptions options) : Handler<Publishing<T>, Unit> {

  protected override Unit ExecuteCore(Publishing<T> publishing) {
    ConcurrentQueue<Exception> exceptions = Pool<ConcurrentQueue<Exception>>.Get();

    try {
      Parallel.ForEach(publishing, options, subscriber => {
        try {
          subscriber.Execute(publishing.arg);
        }
        catch (Exception e) {
          exceptions.Enqueue(e);
        }
      });

      switch (exceptions.Count) {
        case > 1:
          throw new AggregateException(exceptions);
        case 1:
          ExceptionDispatchInfo.Capture(exceptions.Single()).Throw();
          break;
      }

      return default;
    }
    finally {
      while (exceptions.TryDequeue(out Exception _)) { }

      Pool<ConcurrentQueue<Exception>>.Return(exceptions);
    }
  }

}