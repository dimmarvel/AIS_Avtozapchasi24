using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class Postavshiki : Form
    {
        private SqlDataAdapter _sqlDataAdapter  = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlConnection _sqlConnection    = null;
        private DataSet _dataSet                = null;
        private bool newRowAdding               = false;

        public Postavshiki()
        {
            InitializeComponent();
        }

        private void Postavshiki_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(Constants._connectStr);
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Комманда] FROM BuyTable", _sqlConnection);
                _sqlBuilder = new SqlCommandBuilder(_sqlDataAdapter); //init

                _sqlBuilder.GetInsertCommand(); //generate command for insert update and delete
                _sqlBuilder.GetUpdateCommand();
                _sqlBuilder.GetDeleteCommand();

                _dataSet = new DataSet();
                _sqlDataAdapter.Fill(_dataSet, "BuyTable");

                dataGridView1.DataSource = _dataSet.Tables["BuyTable"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkCell;
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
                _dataSet.Tables["BuyTable"].Clear();

                _sqlDataAdapter.Fill(_dataSet, "BuyTable");

                dataGridView1.DataSource = _dataSet.Tables["BuyTable"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);
                            _dataSet.Tables["BuyTable"].Rows[rowIndex].Delete();
                            _sqlDataAdapter.Update(_dataSet, "BuyTable");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = _dataSet.Tables["BuyTable"].NewRow();

                        row["Запчасть"] = dataGridView1.Rows[rowIndex].Cells["Запчасть"].Value;
                        row["Марка"] = dataGridView1.Rows[rowIndex].Cells["Марка"].Value;
                        row["Изготовитель"] = dataGridView1.Rows[rowIndex].Cells["Изготовитель"].Value;
                        row["Цена_за_шт"] = dataGridView1.Rows[rowIndex].Cells["Цена_за_шт"].Value;

                        _dataSet.Tables["BuyTable"].Rows.Add(row);
                        _dataSet.Tables["BuyTable"].Rows.RemoveAt(_dataSet.Tables["BuyTable"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";
                        _sqlDataAdapter.Update(_dataSet, "BuyTable");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        _dataSet.Tables["BuyTable"].Rows[r]["Запчасть"] = dataGridView1.Rows[r].Cells["Запчасть"].Value;
                        _dataSet.Tables["BuyTable"].Rows[r]["Марка"] = dataGridView1.Rows[r].Cells["Марка"].Value;
                        _dataSet.Tables["BuyTable"].Rows[r]["Изготовитель"] = dataGridView1.Rows[r].Cells["Изготовитель"].Value;
                        _dataSet.Tables["BuyTable"].Rows[r]["Цена_за_шт"] = dataGridView1.Rows[r].Cells["Цена_за_шт"].Value;

                        _sqlDataAdapter.Update(_dataSet, "BuyTable");
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    dataGridView1[5, lastRow] = linkCell;
                    row.Cells["Команда"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, rowIndex] = linkCell;
                    editingRow.Cells["Команда"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }
    }
}
