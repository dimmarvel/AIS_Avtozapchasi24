using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class Storage : Form
    {
        private SqlConnection _sqlConnection = null;
        private SqlCommandBuilder _sqlBuilder = null;
        private SqlDataAdapter _sqlDataAdapter = null;
        private DataSet _dataSet = null;

        public Storage()
        {
            InitializeComponent();
        }

        private void Storage_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Products", _sqlConnection);

                _sqlBuilder = new SqlCommandBuilder(_sqlDataAdapter); //init

                _dataSet = new DataSet(); //init

                _sqlDataAdapter.Fill(_dataSet, "Products"); // заполнение
                dataGridView1.DataSource = _dataSet.Tables["Products"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
