using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace AdminPanel
{
    internal class EditForm : MetroForm
    {
        private MetroTextBox _metroTextBox1;
        private MetroButton _metroButton1;
        private string _content;

        public string Content { get { return _metroTextBox1.Text; } }

        public EditForm(string title, string content)
        {
            InitializeComponent();
            Text = title;
            _content = content.Replace("\n", "\r\n");
            _metroTextBox1.Text = _content;
        }

        private void InitializeComponent()
        {
            _metroTextBox1 = new MetroTextBox();
            _metroButton1 = new MetroButton();
            SuspendLayout();
            // 
            // metroTextBox1
            // 
            _metroTextBox1.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) 
                                    | AnchorStyles.Left) 
                                   | AnchorStyles.Right;
            _metroTextBox1.Lines = new[] {
        "metroTextBox1"};
            _metroTextBox1.Location = new Point(24, 64);
            _metroTextBox1.MaxLength = 32767;
            _metroTextBox1.Multiline = true;
            _metroTextBox1.Name = "_metroTextBox1";
            _metroTextBox1.PasswordChar = '\0';
            _metroTextBox1.ScrollBars = ScrollBars.Both;
            _metroTextBox1.SelectedText = "";
            _metroTextBox1.Size = new Size(453, 364);
            _metroTextBox1.TabIndex = 0;
            _metroTextBox1.Text = "metroTextBox1";
            _metroTextBox1.UseSelectable = true;
            // 
            // metroButton1
            // 
            _metroButton1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left) 
                                  | AnchorStyles.Right;
            _metroButton1.Location = new Point(23, 434);
            _metroButton1.Name = "_metroButton1";
            _metroButton1.Size = new Size(454, 37);
            _metroButton1.TabIndex = 1;
            _metroButton1.Text = "Save Settings";
            _metroButton1.UseSelectable = true;
            _metroButton1.Click += metroButton1_Click;
            // 
            // EditForm
            // 
            ClientSize = new Size(500, 494);
            Controls.Add(_metroButton1);
            Controls.Add(_metroTextBox1);
            Name = "EditForm";
            Text = "Edit";
            ResumeLayout(false);

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}