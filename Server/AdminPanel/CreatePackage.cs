using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using MetroFramework.Forms;
using Newtonsoft.Json;

namespace AdminPanel
{
    public partial class CreatePackage : MetroForm
    {
        private string _jsonResult;

        public CreatePackage()
        {
            InitializeComponent();
        }

        private void removeItem_Click(object sender, EventArgs e)
        {
            if (itemsList.SelectedItem != null)
                itemsList.Items.Remove(itemsList.SelectedItem);
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            int count;
            int item;

            if (new AddItemForm().ShowDialog(out count, out item) != DialogResult.OK) return;
            for (int i = 0; i < count; i++)
                itemsList.Items.Add(item);
        }

        private void saveBox_Click(object sender, EventArgs e)
        {
            var package = new Package
            {
                Name = packageName.Text,
                MaxPurchase = int.Parse(maxPurchase.Text),
                Weight = int.Parse(weight.Text),
                Content = new Content
                {
                    Items = itemsList.Items.Cast<int>().ToArray(),
                    CharSlots = int.Parse(charSlotsBox.Text),
                    VaultChests = int.Parse(vaultChestsBox.Text)
                },
                BgUrl = bgUrl.Text,
                Price = int.Parse(price.Text),
                Quantity = int.Parse(quantity.Text),
                EndDate = db.Database.DateTimeToUnixTimestamp(endDate.Value)
            };

            JsonSerializer s = new JsonSerializer();
            var wtr = new StringWriter();
            s.Serialize(wtr, package, typeof(Package));
            _jsonResult = wtr.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }

        public string PackageResult => HttpUtility.UrlEncode(_jsonResult);
    }

    struct Package
    {
        public string Name;
        public int MaxPurchase;
        public int Weight;
        public Content Content;
        public string BgUrl;
        public int Price;
        public int Quantity;
        public int EndDate;
    }

    struct Content
    {
        public int[] Items;
        public int CharSlots;
        public int VaultChests;
    }
}
