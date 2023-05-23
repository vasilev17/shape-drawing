using System;
using System.Drawing;

namespace ShapeDrawing.Shape_Classes
{
    public class Elipse : Shape
    {
        public Elipse(Color color, Point location, Size size) : base(color, location, size) { }

        public Elipse() { }

        public override decimal CalculateArea() => (decimal)Math.PI * Size.Width * Size.Height;

        public override void Draw(Graphics graphics) => graphics.FillEllipse(Brush, Location.X, Location.Y, Size.Width, Size.Height);
    }
}
