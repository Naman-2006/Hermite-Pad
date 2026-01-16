using System;
using System.Collections.Generic;

namespace HermitePad.Core
{
    public class UndoRedoManager
    {
        private readonly Stack<IAction> _undoStack;
        private readonly Stack<IAction> _redoStack;
        private const int MaxStackSize = 100;

        public UndoRedoManager()
        {
            _undoStack = new Stack<IAction>();
            _redoStack = new Stack<IAction>();
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void AddAction(IAction action)
        {
            _undoStack.Push(action);
            _redoStack.Clear();

            // Limit stack size
            if (_undoStack.Count > MaxStackSize)
            {
                var temp = new List<IAction>(_undoStack);
                temp.RemoveAt(temp.Count - 1);
                _undoStack.Clear();
                foreach (var a in temp)
                {
                    _undoStack.Push(a);
                }
            }
        }

        public void Undo()
        {
            if (CanUndo)
            {
                var action = _undoStack.Pop();
                action.Undo();
                _redoStack.Push(action);
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                var action = _redoStack.Pop();
                action.Redo();
                _undoStack.Push(action);
            }
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }

    public interface IAction
    {
        void Undo();
        void Redo();
    }
}
