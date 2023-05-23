using System.Collections.Generic;

namespace ShapeDrawing.Command_Classes
{
    class RemoveAllCommand : Interfaces.IUndoableRedoable
    {
        private List<Shape_Classes.Shape> drawnShapes;
        private List<Shape_Classes.Shape> oldDrawnShapes = new List<Shape_Classes.Shape>();

        public RemoveAllCommand(List<Shape_Classes.Shape> drawnShapes)
        {
            this.oldDrawnShapes.AddRange(drawnShapes);
            this.drawnShapes = drawnShapes;

        }

        public void Execute() => drawnShapes.Clear();

        public void UndoExecution() => drawnShapes.AddRange(oldDrawnShapes);

    }
}
