using JetBrains.Annotations;

namespace Interactions.Tests.Executables;

[TestSubject(typeof(Executable))]
public class ExecutableCreationTest {

  [Fact]
  public void ThrowArgumentNullException() {
    Assert.Throws<ArgumentNullException>(() => Executable.Create(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit, Unit>(null));

    Assert.Throws<ArgumentNullException>(() => Executable.Create(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create((Action<Unit>)null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit>(null));
    Assert.Throws<ArgumentNullException>(() => Executable.Create<Unit, Unit>(null));
  }

}