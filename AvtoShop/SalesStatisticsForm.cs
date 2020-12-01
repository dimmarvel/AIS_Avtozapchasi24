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
    public partial class SalesStatisticsForm : Form
    {
        private SqlDataAdapter _sqlDataAdapter  = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlConnection _sqlConnection    = null;
        private SqlCommand _sqlCommand          = null;
        private DataSet _dataSet                = null;

        public SalesStatisticsForm()
        {
            InitializeComponent();
        }

        private void SalesStatisticsForm_Load(object sender, EventArgs e)
        {
            set_date();
            _sqlConnection = new SqlConnection(Constants._connectStr);
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }

        private void set_date()
        {
            dateTimePicker1.Format          = DateTimePickerFormat.Custom;
            dateTimePicker2.Format          = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat    = "dd.MM.yyyy hh:mm";
            dateTimePicker2.CustomFormat    = "dd.MM.yyyy hh:mm";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM SoldStat " + "WHERE [Дата_продажи] BETWEEN N'"
                           + dateTimePicker1.Text + "' AND N'" + dateTimePicker2.Text + "'";
                _sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataReader dr = _sqlCommand.ExecuteReader();
                DataTable dt = new DataTable();

                dt.Load(dr);
                dataGridView1.DataSource = dt;
                MessageBox.Show("Таблица успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT * FROM SoldStat", _sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init
                _dataSet        = new DataSet(); //init

                _sqlDataAdapter.Fill(_dataSet, "SoldStat"); // заполнение
                dataGridView1.DataSource = _dataSet.Tables["SoldStat"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
