using System.Text;
using AutoFixture;
using Executables.Commands;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Commands;

[TestSubject(typeof(CommandContext))]
public class CommandContextTest(ITestOutputHelper testOutputHelper) {

  [Fact]
  public void UndoRedoTest() {
    var fixture = new Fixture();
    var objects = new List<object>();

    var context = new CommandContext();

    ICommand<int> numCommand = context.CreateCommand<int>(
      num => objects.Add(num),
      num => objects.Remove(num)
    );

    ICommand<string> stringCommand = context.CreateCommand<string>(
      s => objects.Add(s),
      s => objects.Remove(s)
    );

    ICommand<bool> boolCommand = context.CreateCommand<bool>(
      b => objects.Add(b),
      b => objects.Remove(b)
    );

    var builder = new StringBuilder();

    testOutputHelper.WriteLine("Start execute");

    numCommand.Execute(fixture.Create<int>());
    DisplayObjects(objects, builder);
    stringCommand.Execute(fixture.Create<string>());
    DisplayObjects(objects, builder);
    boolCommand.Execute(fixture.Create<bool>());
    DisplayObjects(objects, builder);

    testOutputHelper.WriteLine(string.Empty);
    testOutputHelper.WriteLine("Start undo");
    while (context.Undo())
      DisplayObjects(objects, builder);

    testOutputHelper.WriteLine(string.Empty);
    testOutputHelper.WriteLine("Start redo");
    while (context.Redo())
      DisplayObjects(objects, builder);

    Assert.True(context.Undo());
    stringCommand.Execute(fixture.Create<string>());
    Assert.False(context.Redo());
  }

  private void DisplayObjects(List<object> objects, StringBuilder builder) {
    for (var i = 0; i < objects.Count - 1; i++)
      builder.Append($"{objects[i]}; ");
    if (objects.Count > 0)
      builder.Append(objects.Last());
    else
      builder.Append(string.Empty);

    testOutputHelper.WriteLine($"Objects: {builder}");
    builder.Clear();
  }

}