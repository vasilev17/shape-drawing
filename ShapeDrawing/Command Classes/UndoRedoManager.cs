using System.Collections.Generic;

namespace ShapeDrawing.Command_Classes
{
    class UndoRedoManager
    {
        private Stack<Interfaces.IUndoableRedoable> undoCommands = new Stack<Interfaces.IUndoableRedoable>();
        private Stack<Interfaces.IUndoableRedoable> redoCommands = new Stack<Interfaces.IUndoableRedoable>();

        public bool hasUndo() => undoCommands.Count > 0 ? true : false;

        public bool hasRedo() => redoCommands.Count > 0 ? true : false;

        public void Undo()
        {
            if (undoCommands.Count != 0)
            {
                Interfaces.IUndoableRedoable command = undoCommands.Pop();
                command.UndoExecution();
                redoCommands.Push(command);
            }

        }
         
        public void Redo()
        {
            if (redoCommands.Count != 0)
            {
                Interfaces.IUndoableRedoable command = redoCommands.Pop();
                command.Execute();
                undoCommands.Push(command);
            }

        }

        public void EmptyManager()
        {
            undoCommands.Clear();
            redoCommands.Clear();
        }

        public void SendCommand(Interfaces.IUndoableRedoable command)
        {
            undoCommands.Push(command);
            redoCommands.Clear();
        }

    }
}
