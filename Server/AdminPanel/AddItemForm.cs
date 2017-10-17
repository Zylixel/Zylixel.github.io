using System;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class AddItemForm : Form
    {
        private int _count;
        private int _item;

        public AddItemForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _count = (int)numericUpDown1.Value;
            _item = (int)numericUpDown2.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        public DialogResult ShowDialog(out int count, out int item)
        {
            var ret = ShowDialog();
            count = _count;
            item = _item;
            return ret;
        }
    }
}
