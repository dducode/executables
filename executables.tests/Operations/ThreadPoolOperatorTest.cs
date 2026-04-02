using Executables.Core.Operators;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Operations;

[TestSubject(typeof(ThreadPoolOperator<>))]
public class ThreadPoolOperatorTest(ITestOutputHelper output) {

  [Fact]
  public async Task InvokeOnThreadPool() {
    var senderThreadTcs = new TaskCompletionSource<(bool IsThreadPoolThread, int ThreadId)>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );
    var executorThreadTcs = new TaskCompletionSource<(bool IsThreadPoolThread, int ThreadId)>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );

    IQuery<string, Unit> query = Executable
      .Create((string message) => {
        output.WriteLine(message);
        executorThreadTcs.TrySetResult((Thread.CurrentThread.IsThreadPoolThread, Environment.CurrentManagedThreadId));
      })
      .OnThreadPool()
      .AsQuery();

    var senderThread = new Thread(() => {
      senderThreadTcs.TrySetResult((Thread.CurrentThread.IsThreadPoolThread, Environment.CurrentManagedThreadId));
      query.Send("Test");
    });

    senderThread.Start();

    (bool IsThreadPoolThread, int ThreadId) senderInfo = await senderThreadTcs.Task;
    (bool IsThreadPoolThread, int ThreadId) executorInfo = await executorThreadTcs.Task;

    senderThread.Join();

    Assert.False(senderInfo.IsThreadPoolThread);
    Assert.True(executorInfo.IsThreadPoolThread);
    Assert.NotEqual(senderInfo.ThreadId, executorInfo.ThreadId);
  }

}
