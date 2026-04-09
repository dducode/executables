using Executables.Core.Executors;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Operations;

[TestSubject(typeof(ThreadPoolExecutor<>))]
public class ThreadPoolExecutorTest(ITestOutputHelper output) {

  [Fact]
  public async Task InvokeOnThreadPool() {
    var senderThreadTcs = new TaskCompletionSource<(bool IsThreadPoolThread, int ThreadId)>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );
    var executorThreadTcs = new TaskCompletionSource<(bool IsThreadPoolThread, int ThreadId)>(
      TaskCreationOptions.RunContinuationsAsynchronously
    );

    IExecutor<string, Unit> executor = Executable
      .Create((string message) => {
        output.WriteLine(message);
        executorThreadTcs.TrySetResult((Thread.CurrentThread.IsThreadPoolThread, Environment.CurrentManagedThreadId));
      })
      .GetExecutor()
      .OnThreadPool();

    var senderThread = new Thread(() => {
      senderThreadTcs.TrySetResult((Thread.CurrentThread.IsThreadPoolThread, Environment.CurrentManagedThreadId));
      executor.Execute("Test");
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