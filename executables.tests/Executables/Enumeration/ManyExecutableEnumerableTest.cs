using Executables.Enumeration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Executables.Enumeration;

[TestSubject(typeof(ManyExecutableEnumerable<,>))]
public class ManyExecutableEnumerableTest(ITestOutputHelper output) {

  [Fact]
  public void EnumeratePlayersItems() {
    List<Player> players = GetPlayers();
    IQuery<Player, List<Item>> query = Executable.Create((Player player) => player.items).AsQuery();

    foreach (Item item in query.ForEachMany(players))
      output.WriteLine($"Item id: {item.id}");
  }

  private List<Player> GetPlayers() {
    return [
      new Player {
        items = [
          new Item { id = 0 },
          new Item { id = 1 },
          new Item { id = 2 }
        ]
      },

      new Player {
        items = [
          new Item { id = 3 }
        ]
      },

      new Player {
        items = [
          new Item { id = 4 },
          new Item { id = 5 }
        ]
      },

      new Player {
        items = []
      },

      new Player {
        items = [
          new Item { id = 6 },
          new Item { id = 7 },
          new Item { id = 8 }
        ]
      }
    ];
  }

  private class Player {

    public List<Item> items;

  }

  private class Item {

    public int id;

  }

}