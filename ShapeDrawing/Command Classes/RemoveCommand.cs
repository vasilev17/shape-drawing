using System.Collections.Generic;

namespace ShapeDrawing.Command_Classes
{
    class RemoveCommand : Interfaces.IUndoableRedoable
    {
        private Shape_Classes.Shape shape;
        private List<Shape_Classes.Shape> drawnShapes;

        public RemoveCommand(Shape_Classes.Shape shape, List<Shape_Classes.Shape> drawnShapes)
        {
            this.shape = shape;
            this.drawnShapes = drawnShapes;
        }

        public void Execute() => drawnShapes.Remove(shape);

        public void UndoExecution() => drawnShapes.Add(shape);

    }
}
