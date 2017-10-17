using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;

namespace AdminPanel
{
    public partial class DatabaseView : MetroForm
    {
        private Database _db;
        private TaskScheduler _currentContext;

        public DatabaseView(string connStr)
        {
            InitializeComponent();
            _db = new Database(connStr);
            _currentContext = TaskScheduler.FromCurrentSynchronizationContext();
            InitAsync();
        }

        private async void InitAsync()
        {
            await _db.OpenAsync();
            var table = new DataTable();
            await Task.Factory.StartNew(() =>
            {
                var cmd = _db.CreateQuery("SELECT * FROM accounts;");
                var da = new MySqlDataAdapter(cmd);
                da.Fill(table);

                accountsTableGrid.CellValueChanged += AccountsTableGrid_CellValueChanged;
                accountsTableGrid.CellContentClick += AccountsTableGrid_CellContentClick;
            }).ContinueWith(t =>
            {
                accountsTableGrid.DataSource = table;
            }, _currentContext);

            table = new DataTable();
            await Task.Factory.StartNew(() =>
            {
                var cmd = _db.CreateQuery("SELECT * FROM characters;");
                var da = new MySqlDataAdapter(cmd);
                da.Fill(table);

                charactersTableGrid.CellValueChanged += CharactersTableGrid_CellValueChanged;
                charactersTableGrid.CellContentClick += CharactersTableGrid_CellContentClick;
            }).ContinueWith(t =>
            {
                charactersTableGrid.DataSource = table;
            }, _currentContext);
        }

        private void CharactersTableGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            charactersTableGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void CharactersTableGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _db.CreateQueryAsync("UPDATE characters SET " + charactersTableGrid.Columns[e.ColumnIndex].Name + "=@value WHERE accId=@accId; AND id=@charId",
                    Database.CreateParameter("@value", charactersTableGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value),
                    Database.CreateParameter("@accId", charactersTableGrid.Rows[e.RowIndex].Cells["accId"].Value),
                    Database.CreateParameter("@charId", charactersTableGrid.Rows[e.RowIndex].Cells["id"].Value));
        }

        private void AccountsTableGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            accountsTableGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void AccountsTableGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _db.CreateQueryAsync("UPDATE accounts SET " + accountsTableGrid.Columns[e.ColumnIndex].Name + "=@value WHERE id=@accId;",
                    Database.CreateParameter("@value", accountsTableGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value),
                    Database.CreateParameter("@accId", accountsTableGrid.Rows[e.RowIndex].Cells["id"].Value));
        }
    }
}
