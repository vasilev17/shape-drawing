using System.Drawing;

namespace ShapeDrawing.Shape_Classes
{
    public class Triangle : Shape
    {
        public Triangle(Color color, Point location, Size size) : base(color, location, size) { }

        public Triangle() { }

        public override decimal CalculateArea() => Size.Width * Size.Height / 2;

        public override void Draw(Graphics graphics) => graphics.FillPolygon(Brush, new Point[3] { new Point(Location.X + Size.Width / 2, Location.Y), new Point(Location.X, Location.Y + Size.Height), new Point(Location.X + Size.Width, Location.Y + Size.Height) });
    }
}
