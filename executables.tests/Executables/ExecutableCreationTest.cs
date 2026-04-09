using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(Executable))]
public class ExecutableCreationTest {

  [Fact]
  public void ThrowArgumentNullException() {
    Assert.Throws<ArgumentNullException>(() => Executable.FlatMap((IExecutable<Unit, IExecutable<Unit, Unit>>)null));
    Assert.Throws<ArgumentNullException>(() => Executable.Accumulate<Unit, Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Fork((IExecutable<Unit, Unit>)null, (IExecutable<Unit, Unit>)null));

    Assert.Throws<ArgumentNullException>(() => Executable.Create(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit, Unit>(null));

    Assert.Throws<ArgumentNullException>(() => Executable.Create(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create((Action<Unit, Unit>)null));
  }

}