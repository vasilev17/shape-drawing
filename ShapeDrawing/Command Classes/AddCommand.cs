using System.Collections.Generic;

namespace ShapeDrawing.Command_Classes
{
    class AddCommand : Interfaces.IUndoableRedoable
    {
        private Shape_Classes.Shape newShape;
        private List<Shape_Classes.Shape> drawnShapes;

        public AddCommand(Shape_Classes.Shape newShape, List<Shape_Classes.Shape> drawnShapes)
        {
            this.newShape = newShape;
            this.drawnShapes = drawnShapes;
        }

        public void Execute()
        {
            drawnShapes.Add(newShape);
        }

        public void UndoExecution()
        {
            drawnShapes.Remove(newShape);
        }
    }
}
