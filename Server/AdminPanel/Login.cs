using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using MetroFramework;
using MetroFramework.Forms;

namespace AdminPanel
{
    public partial class Login : MetroForm
    {
        private Account _account;

        public Login()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(out Account user)
        {
            var ret = ShowDialog();
            user = _account;
            return ret;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var webRequest = WebRequest.CreateHttp(String.Format("http://127.0.0.1/account/verify?guid={0}&password={1}", emailTextBox.Text, passwordTextBox.Text));
                using (StreamReader rdr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    var xml = rdr.ReadToEnd();
                    if (xml == "<Error>WebChangePasswordDialog.passwordError</Error>")
                    {
                        DialogResult = DialogResult.None;
                        MetroMessageBox.Show(this, "\n\nAccount credentials not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var serializer = new XmlSerializer(typeof(Account));
                    _account = (Account)serializer.Deserialize(new StringReader(xml));

                    if (!_account.Admin)
                    {
                        DialogResult = DialogResult.None;
                        MetroMessageBox.Show(this, "\n\nYou are not an admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _account.Password = passwordTextBox.Text;
                    _account.Email = emailTextBox.Text;
                    DialogResult = DialogResult.OK;
                }
            }
            catch (WebException)
            {
                MetroMessageBox.Show(this, "\n\nUnable to contact server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
