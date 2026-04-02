using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(ExecutableExtensions))]
public class ExecutableExtensionsTest {

  [Fact]
  public void ThrowNullArgumentException() {
    Assert.Throws<NullReferenceException>(() => ((IExecutable<Unit, Unit>)null).Then(Executable.Identity()));

    IExecutable<Unit, Unit> identity = Executable.Identity();

    Assert.Throws<ArgumentNullException>(() => identity.Then((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Then((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Then((IAsyncExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Then((AsyncFunc<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Fork((IExecutable<Unit, Unit>)null, (IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Fork((Func<Unit, Unit>)null, (Func<Unit, Unit>)null));

    IExecutable<Unit, (Unit, Unit)> fork = identity.Fork(Executable.Identity(), Executable.Identity());
    Assert.Throws<ArgumentNullException>(() => fork.First((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.First((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Second((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Second((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Merge((IExecutable<(Unit, Unit), Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Merge((Func<Unit, Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Map((IExecutable<Unit, Unit>)null, (IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Map((Func<Unit, Unit>)null, (Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.InMap((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.OutMap((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.InMap((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.OutMap((Func<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Tap((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Tap(null));
  }

  [Fact]
  public void ThrowNullArgumentExceptionAsync() {
    Assert.Throws<NullReferenceException>(() => ((IAsyncExecutable<Unit, Unit>)null).Then(Executable.Identity()));

    IAsyncExecutable<Unit, Unit> identity = Executable.Identity().ToAsyncExecutable();

    Assert.Throws<ArgumentNullException>(() => identity.Then((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Then((IAsyncExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Then((AsyncFunc<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Fork((IAsyncExecutable<Unit, Unit>)null, (IAsyncExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Fork((AsyncFunc<Unit, Unit>)null, (AsyncFunc<Unit, Unit>)null));

    IAsyncExecutable<Unit, (Unit, Unit)> fork = identity.Fork(Executable.Identity().ToAsyncExecutable(), Executable.Identity().ToAsyncExecutable());
    Assert.Throws<ArgumentNullException>(() => fork.First((IAsyncExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.First((AsyncFunc<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Second((IAsyncExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Second((AsyncFunc<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Merge((IAsyncExecutable<(Unit, Unit), Unit>)null));
    Assert.Throws<ArgumentNullException>(() => fork.Merge((AsyncFunc<Unit, Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Map((IExecutable<Unit, Unit>)null, (IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Map((Func<Unit, Unit>)null, (Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.InMap((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.OutMap((IExecutable<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.InMap((Func<Unit, Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.OutMap((Func<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => identity.Tap((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => identity.Tap(null));
  }

}