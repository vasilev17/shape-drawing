using System.Drawing;

namespace ShapeDrawing.Command_Classes
{
    class EditCommand : Interfaces.IUndoableRedoable
    {
        private Shape_Classes.Shape shape;
        private Size newSize, oldSize;
        private Color newColor, oldColor;

        public EditCommand(Shape_Classes.Shape shape, Size newSize, Color newColor)
        {
            oldSize = shape.Size;
            oldColor = shape.Color;

            this.shape = shape;
            this.newSize = newSize;
            this.newColor = newColor;
        }

        public void Execute()
        {
            shape.Size = newSize;
            shape.Color = newColor;
        }

        public void UndoExecution()
        {
            shape.Size = oldSize;
            shape.Color = oldColor;
        }
    }
}
