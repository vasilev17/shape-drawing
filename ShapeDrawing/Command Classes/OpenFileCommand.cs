using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ShapeDrawing.Command_Classes
{
    class OpenFileCommand : Interfaces.ICommand
    {
        private List<Shape_Classes.Shape> drawnShapes;
        private string filePath;


        public OpenFileCommand( List<Shape_Classes.Shape> drawnShapes, string filePath)
        {
            this.filePath = filePath;
            this.drawnShapes = drawnShapes;
        }

        public void Execute()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(List<Shape_Classes.Shape>));
                reader = new StreamReader(filePath);
                drawnShapes.AddRange((List<Shape_Classes.Shape>)serializer.Deserialize(reader));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

        }

    }

}
