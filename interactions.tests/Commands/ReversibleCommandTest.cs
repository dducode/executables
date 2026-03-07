using System.Text;
using AutoFixture;
using Interactions.Commands;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Commands;

[TestSubject(typeof(ReversibleCommand<,>))]
public class ReversibleCommandTest(ITestOutputHelper testOutputHelper) {

  [Fact]
  public void AddNumsToListTest() {
    var fixture = new Fixture();
    var nums = new List<int>();

    var command = new ReversibleCommand<int, int>();
    command.Handle(ReversibleHandler.Create<int>(
      num => nums.Add(num),
      num => nums.Remove(num)
    ));

    var builder = new StringBuilder();

    testOutputHelper.WriteLine("Start execute");

    for (var i = 0; i < 5; i++) {
      command.Execute(fixture.Create<int>());
      DisplayNumbers(nums, builder);
    }

    int maxCount = nums.Count;

    testOutputHelper.WriteLine("\nStart undo");
    while (command.Undo())
      DisplayNumbers(nums, builder);

    Assert.Empty(nums);

    testOutputHelper.WriteLine("\nStart redo");
    while (command.Redo())
      DisplayNumbers(nums, builder);

    Assert.Equal(nums.Count, maxCount);

    Assert.True(command.Undo());
    command.Execute(fixture.Create<int>());
    Assert.False(command.Redo());
  }

  [Fact]
  public void AssigneeVariableTest() {
    var fixture = new Fixture();
    var variable = 0;
    int startValue = variable;

    var command = new ReversibleCommand<int, Change<int>>();
    command.Handle(ReversibleHandler.Create<int, Change<int>>(num => {
        var change = new Change<int>(variable, num);
        variable = num;
        return change;
      }, change => variable = change.Old,
      change => variable = change.New
    ));

    testOutputHelper.WriteLine("Start execute");

    for (var i = 0; i < 5; i++) {
      command.Execute(fixture.Create<int>());
      testOutputHelper.WriteLine($"Variable: {variable}");
    }

    int endValue = variable;

    testOutputHelper.WriteLine("\nStart undo");
    while (command.Undo())
      testOutputHelper.WriteLine($"Variable: {variable}");

    Assert.Equal(variable, startValue);

    testOutputHelper.WriteLine("\nStart redo");
    while (command.Redo())
      testOutputHelper.WriteLine($"Variable: {variable}");

    Assert.Equal(variable, endValue);
  }

  private void DisplayNumbers(List<int> nums, StringBuilder builder) {
    for (var i = 0; i < nums.Count - 1; i++)
      builder.Append($"{nums[i]}; ");
    if (nums.Count > 0)
      builder.Append(nums.Last());
    else
      builder.Append(string.Empty);

    testOutputHelper.WriteLine($"Numbers: {builder}");
    builder.Clear();
  }

}