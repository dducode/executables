using Executables.Internal.Extensions;

namespace Executables;

/// <summary>
/// Stores a bounded sequence of values with undo and redo traversal.
/// </summary>
/// <typeparam name="T">Type of value stored in history.</typeparam>
public sealed class History<T>(int capacity = 256) : IDisposable {

  /// <summary>
  /// Gets the total number of values currently tracked by undo and redo stacks.
  /// </summary>
  public int Count => _undoStack.Count + _redoStack.Count;

  private readonly Stack<T> _undoStack = new(capacity);
  private readonly Stack<T> _redoStack = new(capacity);
  private readonly Stack<T> _buffer = new(capacity);

  private bool _disposed;

  /// <summary>
  /// Writes a new value to history and clears redo state.
  /// </summary>
  /// <param name="value">Value to append to history.</param>
  public void Write(T value) {
    ThrowIfDisposed();
    _redoStack.Clear();
    _undoStack.Push(value);
  }

  /// <summary>
  /// Moves one value from undo history to redo history.
  /// </summary>
  /// <param name="value">Receives the value returned by the undo operation.</param>
  /// <returns><see langword="true"/> when a value was undone; otherwise <see langword="false"/>.</returns>
  public bool Undo(out T value) {
    ThrowIfDisposed();

    if (!_undoStack.TryPop(out value))
      return false;

    _redoStack.Push(value);
    return true;
  }

  /// <summary>
  /// Moves one value from redo history back to undo history.
  /// </summary>
  /// <param name="value">Receives the value returned by the redo operation.</param>
  /// <returns><see langword="true"/> when a value was restored; otherwise <see langword="false"/>.</returns>
  public bool Redo(out T value) {
    ThrowIfDisposed();

    if (!_redoStack.TryPop(out value))
      return false;

    _undoStack.Push(value);
    return true;
  }

  /// <summary>
  /// Removes up to the specified number of oldest tracked values from history.
  /// </summary>
  /// <param name="count">Maximum number of values to remove.</param>
  /// <param name="clearedElements">Optional collection that receives removed values.</param>
  public void Clear(int count, ICollection<T> clearedElements = null) {
    if (_disposed)
      return;

    int redoStackCount = _redoStack.Count;
    while (_redoStack.TryPop(out T value))
      _undoStack.Push(value);

    int targetSize = Math.Max(Count - count, 0);

    if (targetSize == 0) {
      clearedElements?.AddRange(_undoStack);
      _undoStack.Clear();
      return;
    }

    while (_undoStack.Count > targetSize)
      _buffer.Push(_undoStack.Pop());

    clearedElements?.AddRange(_undoStack);
    _undoStack.Clear();

    while (_buffer.TryPop(out T value))
      _undoStack.Push(value);

    for (var i = 0; i < redoStackCount; i++)
      _redoStack.Push(_undoStack.Pop());
  }

  /// <summary>
  /// Clears the entire history.
  /// </summary>
  /// <param name="clearedElements">Optional collection that receives removed values.</param>
  public void Clear(ICollection<T> clearedElements = null) {
    if (_disposed)
      return;

    clearedElements?.AddRange(_undoStack);
    _undoStack.Clear();
    clearedElements?.AddRange(_redoStack);
    _redoStack.Clear();
  }

  /// <summary>
  /// Releases history state and prevents further use.
  /// </summary>
  public void Dispose() {
    if (_disposed)
      return;

    Clear();
    _disposed = true;
  }

  private void ThrowIfDisposed() {
#if NET9_0_OR_GREATER
    ObjectDisposedException.ThrowIf(_disposed, this);
#else
    if (_disposed)
      throw new ObjectDisposedException(nameof(History<T>));
#endif
  }

}