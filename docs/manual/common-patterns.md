# Common Patterns

## Building a Processing Pipeline

```csharp
IQuery<string, string> normalizeAndFormat =
  Executable.Create((string text) => text.Trim())
    .Then(text => text.ToUpperInvariant())
    .Then(text => $"Value: {text}")
    .AsQuery();
```

## Wrapping Existing Delegates

```csharp
Func<int, bool> canProcess = value => value > 0;
IExecutable<int, bool> executable = Executable.Create(canProcess);

ICommand<int> command = executable.AsCommand();
Handler<int, bool> handler = executable.AsHandler();
```

## Adding Cross-Cutting Rules

```csharp
IExecutor<string, Result<int>> parse =
  Executable.Create((string text) => int.Parse(text))
    .GetExecutor()
    .WithPolicy(policy => policy
      .ValidateInput(text => !string.IsNullOrWhiteSpace(text), "Value is required")
      .Fallback<FormatException>((text, ex) => 0))
    .WithResult();
```

## Event-Driven Notification

```csharp
var changed = new Event<string>();

changed.Handle(EventPublisher.Sequential<string>());
changed.Subscribe(value => Console.WriteLine($"A: {value}"));
changed.Subscribe(value => Console.WriteLine($"B: {value}"));

changed.Publish("Updated");
```

## Undo and Redo

```csharp
int current = 0;
var assign = new ReversibleCommand<int, Change<int>>();

assign.Handle(ReversibleHandler.Create<int, Change<int>>(
  execution: value =>
  {
    var change = new Change<int>(current, value);
    current = value;
    return change;
  },
  undo: change => current = change.Old,
  redo: change => current = change.New));
```

## UI or Application-Service Integration

```csharp
public readonly record struct User(int Id, string Name);

var users = new Dictionary<int, User>();

ICommand<User> saveUser =
  Executable.Create((User user) =>
  {
    if (string.IsNullOrWhiteSpace(user.Name))
      return false;

    users[user.Id] = user;
    return true;
  })
  .AsCommand();

IQuery<int, User> loadUser =
  Executable.Create((int id) => users.TryGetValue(id, out User user) ? user : throw new UnknownUserException())
  .AsQuery();
```
