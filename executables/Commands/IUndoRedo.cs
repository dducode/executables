namespace Executables.Commands;

public interface IUndoRedo {

  bool Undo();
  bool Redo();
  void ClearHistory();

}