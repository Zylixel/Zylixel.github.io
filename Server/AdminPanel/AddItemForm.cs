using System;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class AddItemForm : Form
    {
        private int count;
        private int item;

        public AddItemForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count = (int)numericUpDown1.Value;
            item = (int)numericUpDown2.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        public DialogResult ShowDialog(out int count, out int item)
        {
            var ret = ShowDialog();
            count = this.count;
            item = this.item;
            return ret;
        }
    }
}
