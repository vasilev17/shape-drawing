using System.Drawing;

namespace ShapeDrawing.Command_Classes
{
    class MoveCommand : Interfaces.IUndoableRedoable
    {
        Point newLocation, oldLocation;
        private Shape_Classes.Shape shape;

        public MoveCommand(Shape_Classes.Shape shape, Point newLocation, Point oldLocation)
        {
            this.oldLocation = oldLocation;

            this.shape = shape;
            this.newLocation = newLocation;

        }

        public void Execute() => shape.Location = newLocation;

        public void UndoExecution() => shape.Location = oldLocation;
    }
}
