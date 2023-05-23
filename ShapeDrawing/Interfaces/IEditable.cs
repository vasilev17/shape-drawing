using System.Drawing;

namespace ShapeDrawing.Interfaces
{
    interface IEditable
    {
        public bool IsSelected { get; set; }

        public bool IsPointInside(Point point);

        public Point GetOffset(Point point);

    }
}
