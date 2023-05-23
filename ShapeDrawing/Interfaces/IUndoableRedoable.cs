namespace ShapeDrawing.Interfaces
{
    interface IUndoableRedoable : ICommand
    {
        void UndoExecution();
    }
}
