using System.Drawing;
using System.Xml.Serialization;

namespace ShapeDrawing.Shape_Classes
{
    [XmlInclude(typeof(Rectangle))]
    [XmlInclude(typeof(Triangle))]
    [XmlInclude(typeof(Elipse))]
    public abstract class Shape : Interfaces.IEditable
    {
        public Point Location { get; set; }

        public Size Size { get; set; }

        [XmlIgnore]
        public Color Color
        {
            get => Brush.Color;
            set => Brush = new SolidBrush(value);
        }

        //Since the Color struct properties are not serializable a surrogate color property is needed
        [XmlElement("Color")]
        public int ColorAsArgb
        {
            get { return Color.ToArgb(); }
            set { Color = Color.FromArgb(value); }
        }

        protected SolidBrush Brush { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; }

        public Shape(Color color, Point location, Size size)
        {
            Brush = new SolidBrush(color);
            Location = location;
            Size = size;
        }

        public Shape() { }

        public abstract void Draw(Graphics graphics);

        public abstract decimal CalculateArea();

        public bool IsPointInside(Point point) =>
            point.X >= Location.X &&
            point.Y >= Location.Y &&
            point.X <= Location.X + Size.Width &&
            point.Y <= Location.Y + Size.Height;

        public Point GetOffset(Point point) => new Point(point.X - Location.X, point.Y - Location.Y);
    }
}
