using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShapeDrawing
{
    public partial class FormShapeEditor : Form
    {
        private Shape_Classes.Shape shapeToEdit;
        Command_Classes.UndoRedoManager commandManager;

        internal FormShapeEditor(Shape_Classes.Shape shapeToEdit, Command_Classes.UndoRedoManager commandManager)
        {
            InitializeComponent();

            heightNumericUpDown.Value = Convert.ToDecimal(shapeToEdit.Size.Height);
            widthNumericUpDown.Value = Convert.ToDecimal(shapeToEdit.Size.Width);
            colorPicker.BackColor = shapeToEdit.Color;

            this.shapeToEdit = shapeToEdit;
            this.commandManager = commandManager;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Interfaces.IUndoableRedoable command = new Command_Classes.EditCommand(shapeToEdit,
                                                   new Size(Convert.ToInt32(widthNumericUpDown.Value), Convert.ToInt32(heightNumericUpDown.Value)),
                                                   colorPicker.BackColor);

            command.Execute();
            commandManager.SendCommand(command);

            DialogResult = DialogResult.OK;
        }

        private void colorPicker_Click(object sender, EventArgs e)
        {

            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorPicker.BackColor = colorDialog.Color;
                this.ActiveControl = null;
            }

        }

        private void cancelButton_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        private void numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Oemcomma) || e.KeyCode.Equals(Keys.OemMinus) || e.KeyCode.Equals(Keys.Subtract))
                e.SuppressKeyPress = true;
        }

    }
}
