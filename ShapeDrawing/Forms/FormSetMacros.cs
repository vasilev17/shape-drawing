using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace ShapeDrawing
{
    partial class FormSetMacros : Form
    {
        List<ToolStripMenuItem> itemsWithMacro = new List<ToolStripMenuItem>();

        internal FormSetMacros(List<ToolStripMenuItem> itemsWithMacro)
        {
            InitializeComponent();

            this.itemsWithMacro = itemsWithMacro;

        }

        private void keyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!keyComboBox.Text.Equals(""))
            {
                okButton.Enabled = true;
                applyButton.Enabled = true;
            }

        }

        private void formSetMacros_Load(object sender, EventArgs e)
        {

            itemsWithMacro.ForEach(item => functionComboBox.Items.Add(item.Text));
            functionComboBox.SelectedIndex = 0;

        }

        private void cancelButton_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        private void applyButton_Click(object sender, EventArgs e) => setShortcut(false);

        private void okButton_Click(object sender, EventArgs e) => setShortcut(true);

        private void setShortcut(bool closeAfter)
        {
            Keys key;
            Enum.TryParse(keyComboBox.Text, out key);
            ToolStripMenuItem item;
            item = itemsWithMacro.Find(item => item.ShortcutKeys == (Keys.Control | key));

            if (item == default(ToolStripMenuItem))
            {
                itemsWithMacro.Find(item => item.Text.Equals(functionComboBox.Text)).ShortcutKeys = Keys.Control | key;

                if (closeAfter)
                    DialogResult = DialogResult.OK;
            }
            else
            {

                if (item.Text != functionComboBox.Text)
                    MessageBox.Show($"This macro is already set for function: \"{item.Text}\"", "Shape Area", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                else if (closeAfter)
                    DialogResult = DialogResult.OK;

            }
        }
    }
}
