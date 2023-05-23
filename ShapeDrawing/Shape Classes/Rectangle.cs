using System.Drawing;

namespace ShapeDrawing.Shape_Classes
{
    public class Rectangle : Shape
    {
        public Rectangle(Color color, Point location, Size size) : base(color, location, size) { }

        public Rectangle() { }

        public override decimal CalculateArea() => Size.Width * Size.Height;

        public override void Draw(Graphics graphics) => graphics.FillRectangle(Brush, Location.X, Location.Y, Size.Width, Size.Height);
    }
}
