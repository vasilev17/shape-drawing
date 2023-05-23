using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShapeDrawing
{

    public partial class MainScene : Form
    {

        public List<Shape_Classes.Shape> drawnShapes = new List<Shape_Classes.Shape>();

        internal Command_Classes.UndoRedoManager commandManager = new Command_Classes.UndoRedoManager();
        private readonly Random _rnd = new Random();


        private Shape_Classes.Shape selectedShape;
        private Point offset;
        private Point startingPoint;
        private Type shapeToDraw;
        private Color colortoDraw = Color.FromArgb(255, 121, 188, 255);

        private int defaultShapeIndex = 1;


        public MainScene() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {

            LoadShapesData();

            IEnumerable<Shape_Classes.Shape> shapes = typeof(Shape_Classes.Shape)
                                              .Assembly.GetTypes()
                                              .Where(t => t.IsSubclassOf(typeof(Shape_Classes.Shape)) && !t.IsAbstract)
                                              .Select(t => (Shape_Classes.Shape)Activator.CreateInstance(t));


            shapes.ToList().ForEach(n => shapesComboBox.Items.Add(n.GetType().Name));

            shapesComboBox.SelectedIndex = defaultShapeIndex;
            shapeToDraw = shapes.ToList()[defaultShapeIndex].GetType();

            colorButton.BackColor = colortoDraw;

        }

        public void CheckManagerUndoRedo()
        {
            if (commandManager.hasUndo() == true)
            {
                undoSymbolToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem.Enabled = true;
            }
            else
            {
                undoSymbolToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = false;
            }


            if (commandManager.hasRedo())
            {
                redoSymbolToolStripMenuItem.Enabled = true;
                redoToolStripMenuItem.Enabled = true;
            }
            else
            {
                redoSymbolToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Enabled = false;
            }


        }

        public void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandManager.Undo();
            Invalidate();
        }

        public void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandManager.Redo();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var drawable in drawnShapes)
                drawable.Draw(e.Graphics);

            CheckManagerUndoRedo();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Size rndSize = new Size(_rnd.Next(50, 200), _rnd.Next(50, 200));

            switch (e.Button)
            {

                case MouseButtons.Left:

                    for (int i = drawnShapes.Count - 1; i >= 0; i--)
                    {
                        if (!drawnShapes[i].IsPointInside(e.Location))
                            continue;

                        selectedShape = drawnShapes[i];
                        startingPoint = selectedShape.Location;
                        offset = selectedShape.GetOffset(e.Location);
                        selectedShape.IsSelected = true;
                        drawnShapes[i] = drawnShapes[drawnShapes.Count - 1];
                        drawnShapes[drawnShapes.Count - 1] = selectedShape;
                        break;
                    }
                    break;

                case MouseButtons.Right:

                    for (int i = drawnShapes.Count - 1; i >= 0; i--)
                    {
                        if (!drawnShapes[i].IsPointInside(e.Location))
                            continue;

                        ContextMenuStrip shapeMenu = new ContextMenuStrip();

                        shapeMenu.Items.AddRange(new ToolStripItem[] {
                            new ToolStripMenuItem("Edit Shape", null, (sender2, e2) => editShapeToolStripMenuItem_Click(i)),
                            new ToolStripMenuItem("Calculate Area", null, (sender2, e2) => calculateAreaToolStripMenuItem_Click(i)),
                            new ToolStripMenuItem("Remove", null, (sender2, e2) => removeToolStripMenuItem_Click(drawnShapes[i]))
                        });

                        shapeMenu.Show(this, e.Location);

                        break;
                    }

                    break;

                case MouseButtons.Middle:

                    Shape_Classes.Shape shape = (Shape_Classes.Shape)Activator.CreateInstance(shapeToDraw);
                    shape.Color = colortoDraw;
                    shape.Location = new Point(e.X, e.Y);
                    shape.Size = rndSize;

                    Interfaces.IUndoableRedoable command = new Command_Classes.AddCommand(shape, drawnShapes);
                    command.Execute();
                    commandManager.SendCommand(command);

                    break;
            }
        }

        private void editShapeToolStripMenuItem_Click(int shapeIndex)
        {
            FormShapeEditor editor = new FormShapeEditor(drawnShapes[shapeIndex], commandManager);
            editor.ShowDialog();
        }

        private void calculateAreaToolStripMenuItem_Click(int shapeIndex) =>
            MessageBox.Show($"Area of the selected {drawnShapes[shapeIndex].GetType().Name} = {FormatArea(drawnShapes[shapeIndex].CalculateArea())}px²", "Shape Area", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void removeToolStripMenuItem_Click(Shape_Classes.Shape shape)
        {
            Interfaces.IUndoableRedoable command = new Command_Classes.RemoveCommand(shape, drawnShapes);
            command.Execute();
            commandManager.SendCommand(command);
        }

        private string FormatArea(decimal area) => area % 1 == 0 ? area.ToString() : area.ToString("0.00");

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            MoveSelected(e.Location);

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (selectedShape == null)
                return;

            Interfaces.IUndoableRedoable command = new Command_Classes.MoveCommand(selectedShape, new Point(e.Location.X - offset.X, e.Location.Y - offset.Y), startingPoint);
            command.Execute();
            commandManager.SendCommand(command);

            selectedShape.IsSelected = false;
            selectedShape = null;
        }

        private void MoveSelected(Point point)
        {
            if (selectedShape == null)
                return;


            selectedShape.Location = new Point(point.X - offset.X, point.Y - offset.Y);
        }

        private void colorButton_Click(object sender, EventArgs e)
        {

            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorButton.BackColor = colorDialog.Color;
                colortoDraw = colorButton.BackColor;
            }

        }

        private void newAndClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Interfaces.IUndoableRedoable command = new Command_Classes.RemoveAllCommand(drawnShapes);
            command.Execute();
            commandManager.SendCommand(command);

            Invalidate();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Normal
                                         ? FormWindowState.Maximized
                                         : FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void setMacrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ToolStripMenuItem> itemsWithMacro = new List<ToolStripMenuItem>();

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                foreach (ToolStripMenuItem subitem in item.DropDownItems)
                {
                    if (item.ShowShortcutKeys == true)
                        itemsWithMacro.Add(item);

                    else if (subitem.ShowShortcutKeys == true)
                        itemsWithMacro.Add(subitem);
                }
            }

            FormSetMacros macroSetter = new FormSetMacros(itemsWithMacro);
            macroSetter.ShowDialog();

        }

        private void shapesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!shapesComboBox.Text.Equals(""))
                shapeToDraw = Type.GetType($"ShapeDrawing.Shape_Classes.{shapesComboBox.Text}");

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Draw Shapes: Click Middle Mouse Button (scroll button)\n" +
                            $"Edit Shapes: Right Mouse Button (on a shape)\n" +
                            $"Move Shapes: Left Mouse Button (drag and drop)",
                            "About...", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text files (*.txt) | *.txt";


            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                Interfaces.IUndoableRedoable removeAllCommand = new Command_Classes.RemoveAllCommand(drawnShapes);
                removeAllCommand.Execute();
                commandManager.SendCommand(removeAllCommand);

                try
                {
                    Interfaces.ICommand openFileCommand = new Command_Classes.OpenFileCommand(drawnShapes, openDialog.FileName);
                    openFileCommand.Execute();
                    commandManager.EmptyManager();

                }
                catch
                {
                    MessageBox.Show($"Problem encountered while opening that file!", "File Opening", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt) | *.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Interfaces.ICommand command = new Command_Classes.SaveFileCommand(drawnShapes, Path.GetFullPath(saveDialog.FileName));
                    command.Execute();
                    MessageBox.Show($"File Saved", "File Saving", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show($"Problem encountered while saving that file!", "File Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void LoadShapesData()
        {
            if (!File.Exists("shapesData"))
                return;

            Interfaces.ICommand openFileCommand = new Command_Classes.OpenFileCommand(drawnShapes, "shapesData");
            openFileCommand.Execute();
        }

        private void MainScene_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Interfaces.ICommand command = new Command_Classes.SaveFileCommand(drawnShapes, "shapesData");
                command.Execute();
            }
            catch
            {
                MessageBox.Show($"Current state could not be saved!", "File Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
