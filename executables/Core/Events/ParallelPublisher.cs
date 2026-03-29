using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using Executables.Events;
using Executables.Handling;
using Executables.Internal;

namespace Executables.Core.Events;

internal sealed class ParallelPublisher<T>(ParallelOptions options) : Handler<Publishing<T>, Unit> {

  protected override Unit HandleCore(Publishing<T> publishing) {
    ConcurrentQueue<Exception> exceptions = Pool<ConcurrentQueue<Exception>>.Get();

    try {
      Parallel.For(0, publishing.subscribers.Count, options, i => {
        try {
          publishing.subscribers[i].Receive(publishing.arg);
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