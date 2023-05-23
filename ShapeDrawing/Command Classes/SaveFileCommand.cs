using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ShapeDrawing.Command_Classes
{
    class SaveFileCommand : Interfaces.ICommand
    {
        private List<Shape_Classes.Shape> drawnShapes;
        private string fileName;


        public SaveFileCommand(List<Shape_Classes.Shape> drawnShapes, string fileName)
        {
            this.drawnShapes = drawnShapes;
            this.fileName = fileName;
        }

        public void Execute()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(List<Shape_Classes.Shape>));
                writer = new StreamWriter(fileName);
                serializer.Serialize(writer, drawnShapes);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

    }
}
