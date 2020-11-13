using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AvtoShop
{
    public partial class ProductsForm : Form
    {
        private SqlDataAdapter _sqlDataAdapter  = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlConnection _sqlConnection    = null;
        private bool newRowAdding               = false;
        private DataSet _dataSet                = null;

        public ProductsForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Products",_sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init

                _sqlBuilder.GetInsertCommand(); //generate command for insert update and delete
                _sqlBuilder.GetUpdateCommand();
                _sqlBuilder.GetDeleteCommand();

                _dataSet = new DataSet();
                _sqlDataAdapter.Fill(_dataSet, "Products");

                dataGridView1.DataSource = _dataSet.Tables["Products"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ReloadData()
        {
            try
            {
                _dataSet.Tables["Products"].Clear();

                _sqlDataAdapter.Fill(_dataSet, "Products");

                dataGridView1.DataSource = _dataSet.Tables["Products"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ProductsForm_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if(task == "Delete")
                    {
                        if(MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);
                            _dataSet.Tables["Products"].Rows[rowIndex].Delete();
                            _sqlDataAdapter.Update(_dataSet, "Products");
                        }
                    }
                    else if(task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = _dataSet.Tables["Products"].NewRow();

                        row["Запчасть"]     = dataGridView1.Rows[rowIndex].Cells["Запчасть"].Value;
                        row["Марка"]        = dataGridView1.Rows[rowIndex].Cells["Марка"].Value;
                        row["Изготовитель"] = dataGridView1.Rows[rowIndex].Cells["Изготовитель"].Value;
                        row["Цена"]         = dataGridView1.Rows[rowIndex].Cells["Цена"].Value;
                        row["Количество"]   = dataGridView1.Rows[rowIndex].Cells["Количество"].Value;

                        _dataSet.Tables["Products"].Rows.Add(row);
                        _dataSet.Tables["Products"].Rows.RemoveAt(_dataSet.Tables["Products"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                        _sqlDataAdapter.Update(_dataSet, "Products");

                        newRowAdding = false;
                    }
                    else if(task == "Update")
                    {
                        int r = e.RowIndex;

                        _dataSet.Tables["Products"].Rows[r]["Запчасть"]     = dataGridView1.Rows[r].Cells["Запчасть"].Value;
                        _dataSet.Tables["Products"].Rows[r]["Марка"]        = dataGridView1.Rows[r].Cells["Марка"].Value;
                        _dataSet.Tables["Products"].Rows[r]["Изготовитель"] = dataGridView1.Rows[r].Cells["Изготовитель"].Value;
                        _dataSet.Tables["Products"].Rows[r]["Цена"]         = dataGridView1.Rows[r].Cells["Цена"].Value;
                        _dataSet.Tables["Products"].Rows[r]["Количество"]   = dataGridView1.Rows[r].Cells["Количество"].Value;

                        _sqlDataAdapter.Update(_dataSet, "Products");
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
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
                if(newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow                     = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row             = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell   = new DataGridViewLinkCell();
                    dataGridView1[6, lastRow]       = linkCell;
                    row.Cells["Command"].Value      = "Insert";
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
                if(newRowAdding == false)
                {
                    int rowIndex                        = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow          = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell       = new DataGridViewLinkCell();
                    dataGridView1[6, rowIndex]          = linkCell;
                    editingRow.Cells["Command"].Value   = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //controll input for Price(only NUMBERS)
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            
            if(dataGridView1.CurrentCell.ColumnIndex == 6)
            {
                TextBox textBox = e.Control as TextBox;

                if(textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        //controll input for Price(only NUMBERS)
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string info = "Функционал формы:\n" +
                "1) Справа есть комманды они делятся на Insert, Delete и Update.\n" +
                "2) По умолчанию стоит комманда Delete, если нажать на команду напротив строки то она удалится.\n" +
                "3) При попытке изменить форму команда Delete заменяется на команду Update при нажатии на неё сохрянятся измененные вами данные в строке в базу данных.\n" +
                "4) При попытке добавить новую запись в пустые строки, напротив записи которую вы хотите добавить появится кнопка Update, после нажатия на которую в базу данных добавится введенная вами строка.";
            MessageBox.Show(info, "Информация о форме", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
