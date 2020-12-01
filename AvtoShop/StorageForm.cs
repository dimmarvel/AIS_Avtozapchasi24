using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class StorageForm : Form
    {
        private SqlDataAdapter _sqlDataAdapter  = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlConnection _sqlConnection    = null;
        private DataSet _dataSet                = null;

        public StorageForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(Constants._connectStr);
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string send = "SELECT * FROM Products " + "WHERE " + comboBox1.Text + " = '" + textBox1.Text + "'";
            
            MessageBox.Show(send);
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = 
                            string.Format($"{comboBox1.Text} = '{textBox1.Text}'");
            MessageBox.Show("Поиск успешно выполнен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Products", _sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init
                _dataSet        = new DataSet(); //init

                _sqlDataAdapter.Fill(_dataSet, "Products"); // заполнение
                dataGridView1.DataSource = _dataSet.Tables["Products"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
