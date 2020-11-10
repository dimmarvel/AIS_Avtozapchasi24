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
    public partial class Basket : Form
    {
        public class BasketTable
        {
            public int      _id;
            public string   _zapchast;
            public string   _mark;
            public string   _izgotovitel;
            public int      _price_per_one;
            public int      _count;

            public void Add_data(int id, string zap, string mark, string izg, int price, int count)
            {
                _id             = id;
                _zapchast       = zap;
                _mark           = mark;
                _izgotovitel    = izg;
                _price_per_one  = price;
                _count          = count;
            }
        }

        private SqlConnection _sqlConnection    = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlDataAdapter _sqlDataAdapter  = null;
        private DataSet _dataSet                = null;
        private List<BasketTable> _basket       = null;
        private bool newRowAdding;

        public Basket()
        {
            InitializeComponent();
        }

        private void Basket_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dimma\Desktop\AvtoShop\AvtoShop\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
            RecalcPrice();
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Комманда] FROM Basket", _sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init
                _dataSet        = new DataSet();

                _sqlBuilder.GetInsertCommand(); //generate command for insert update and delete
                _sqlBuilder.GetUpdateCommand();
                _sqlBuilder.GetDeleteCommand();

                _sqlDataAdapter.Fill(_dataSet, "Basket");

                dataGridView1.DataSource = _dataSet.Tables["Basket"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                _dataSet.Tables["Basket"].Clear();
                _sqlDataAdapter.Fill(_dataSet, "Basket");
                dataGridView1.DataSource = _dataSet.Tables["Basket"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);
                            _dataSet.Tables["Basket"].Rows[rowIndex].Delete();
                            _sqlDataAdapter.Update(_dataSet, "Basket");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = _dataSet.Tables["Basket"].NewRow();

                        row["Запчасть"] = dataGridView1.Rows[rowIndex].Cells["Запчасть"].Value;
                        row["Марка"] = dataGridView1.Rows[rowIndex].Cells["Марка"].Value;
                        row["Изготовитель"] = dataGridView1.Rows[rowIndex].Cells["Изготовитель"].Value;
                        row["Цена"] = dataGridView1.Rows[rowIndex].Cells["Цена"].Value;
                        row["Количество"] = dataGridView1.Rows[rowIndex].Cells["Количество"].Value;

                        _dataSet.Tables["Basket"].Rows.Add(row);
                        _dataSet.Tables["Basket"].Rows.RemoveAt(_dataSet.Tables["Basket"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                        _sqlDataAdapter.Update(_dataSet, "Basket");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        _dataSet.Tables["Basket"].Rows[r]["Запчасть"] = dataGridView1.Rows[r].Cells["Запчасть"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Марка"] = dataGridView1.Rows[r].Cells["Марка"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Изготовитель"] = dataGridView1.Rows[r].Cells["Изготовитель"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Цена"] = dataGridView1.Rows[r].Cells["Цена"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Количество"] = dataGridView1.Rows[r].Cells["Количество"].Value;

                        _sqlDataAdapter.Update(_dataSet, "Basket");
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, lastRow] = linkCell;
                    row.Cells["Комманда"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex                        = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow          = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell       = new DataGridViewLinkCell();
                    dataGridView1[6, rowIndex]          = linkCell;
                    editingRow.Cells["Комманда"].Value  = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadData();
            RecalcPrice();
        }

        private void RecalcPrice()
        {
            SqlCommand sqlCommand       = new SqlCommand("SELECT * FROM Basket", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int price = 0;

            while (sqlDataReader.Read())
                price += (int)sqlDataReader[4] * (int)sqlDataReader[5];

            priceLabel.Text = price.ToString();
            sqlDataReader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadDataToClass();

        }

        private void ReadDataToClass()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Basket", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            _basket = new List<BasketTable>(); // update

            while (sqlDataReader.Read())
            {
                BasketTable bt = new BasketTable();

                bt.Add_data((int)sqlDataReader[0],      sqlDataReader[1].ToString(),
                            sqlDataReader[2].ToString(),sqlDataReader[3].ToString(),
                            (int)sqlDataReader[4], (int)sqlDataReader[5]);

                _basket.Add(bt);
            }

            sqlDataReader.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e){}
    }
}
