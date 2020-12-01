using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class Basket : Form
    {
        private SqlConnection _sqlConnection    = null;
        private SqlCommandBuilder _sqlBuilder   = null;
        private SqlCommand _sqlCommand          = null;
        private SqlDataAdapter _sqlDataAdapter  = null;
        private DataSet _dataSet                = null;
        private List<DataBase> _basket          = null;
        private bool newRowAdding;

        public Basket()
        {
            InitializeComponent();
        }

        private void Basket_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(Constants._connectStr);
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

                        row["Запчасть"]     = dataGridView1.Rows[rowIndex].Cells["Запчасть"].Value;
                        row["Марка"]        = dataGridView1.Rows[rowIndex].Cells["Марка"].Value;
                        row["Изготовитель"] = dataGridView1.Rows[rowIndex].Cells["Изготовитель"].Value;
                        row["Цена"]         = dataGridView1.Rows[rowIndex].Cells["Цена"].Value;
                        row["Количество"]   = dataGridView1.Rows[rowIndex].Cells["Количество"].Value;

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

                        _dataSet.Tables["Basket"].Rows[r]["Запчасть"]       = dataGridView1.Rows[r].Cells["Запчасть"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Марка"]          = dataGridView1.Rows[r].Cells["Марка"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Изготовитель"]   = dataGridView1.Rows[r].Cells["Изготовитель"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Цена"]           = dataGridView1.Rows[r].Cells["Цена"].Value;
                        _dataSet.Tables["Basket"].Rows[r]["Количество"]     = dataGridView1.Rows[r].Cells["Количество"].Value;

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

            if (dataGridView1.CurrentCell.ColumnIndex == 6)
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

        void create_check()
        {
            int counter = 0;
            double sum = 0;
            var builder = new StringBuilder();

            builder.AppendLine($"{"".PadRight(25, ' ')}ОАО Автозапчасти");
            foreach (var product in _basket)
            {
                counter++;
                int sum_zapchast_price = product._price_per_one * product._count;
                sum += sum_zapchast_price;
                builder.AppendLine($"{counter}.{product._zapchast} x{product._count}");
                builder.AppendLine($"  Код:{product._id}");
                builder.AppendLine($"  Стоимость{"".PadRight(40 - sum_zapchast_price.ToString().Length, '.')}{sum_zapchast_price}");
            }
            builder.AppendLine("".PadRight(51, '='));
            builder.AppendLine($"Всего{"".PadRight(46 - sum.ToString().Length, '.')}{sum}");
            builder.AppendLine($"Дата и время: {"".PadRight(46 - DateTime.Now.ToString().Length, '.')}{DateTime.Now}");
            builder.AppendLine($"Итог{"".PadRight(47 - sum.ToString().Length, ' ')}{sum}");
            File.WriteAllText("cheque.txt", builder.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(CheckingBasket() == false) return;

            DataBase _products = new DataBase();

            try
            {
                ReadDataToList("SELECT * FROM Basket",ref _basket);
                string queryProducts = "";

                for (int i = 0; i < _basket.Count; i++)
                {
                    DataBase bd_products = null;
                    if ((bd_products = CheckExistenceTable(_basket[i], "Products")) != null)
                    {
                        if (bd_products._count == _basket[i]._count)
                            queryProducts = "DELETE Products " + "WHERE Id = " + bd_products._id + ";";
                        else if (bd_products._count > _basket[i]._count)
                            queryProducts = "UPDATE Products SET " + "Количество = '" +
                                      (bd_products._count - _basket[i]._count) + "' WHERE Id = " + bd_products._id + ";";

                        string queryStat = "INSERT INTO SoldStat (Запчасть, Марка, Изготовитель, Цена, Продано_шт, Дата_Продажи)" + " VALUES(N'" +
                                    _basket[i]._zapchast + "', N'" +
                                    _basket[i]._mark + "', N'" +
                                    _basket[i]._izgotovitel + "', '" +
                                    _basket[i]._price_per_one + "', '" +
                                    _basket[i]._count + "', '" +
                                    DateTime.Now + "');";
                        create_check();
                        ExecuteAnyQuery(queryProducts, _sqlConnection);
                        ExecuteAnyQuery(queryStat, _sqlConnection);
                        ReadDataToList("SELECT * FROM Basket", ref _basket);
                    }
                }

                ExecuteAnyQuery("DELETE FROM Basket", _sqlConnection); //clear basket

                MessageBox.Show("Товары успешно проданы.\n", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ExecuteAnyQuery(string query, SqlConnection connect)
        {
            _sqlCommand = new SqlCommand(query, connect); //clear basket
            _sqlCommand.ExecuteNonQuery();
        }

        private void ReadDataToList(string query, ref List<DataBase> db)
        {
            SqlCommand sqlCommand       = new SqlCommand(query, _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            db  = new List<DataBase>();

            while (sqlDataReader.Read())
            {
                DataBase bt = new DataBase();

                bt.Add_data((int)sqlDataReader[0],      sqlDataReader[1].ToString(),
                            sqlDataReader[2].ToString(),sqlDataReader[3].ToString(),
                            (int)sqlDataReader[4],      (int)sqlDataReader[5]);

                db.Add(bt);
            }

            sqlDataReader.Close();
        }

        private bool CheckingBasket() //Checking the correctness of the basket
        {
            ReadDataToList("SELECT * FROM Basket", ref _basket);

            for (int i = 0; i < _basket.Count; i++)
            {
                DataBase bd = null;
                if ((bd = CheckExistenceTable(_basket[i], "Products")) != null)
                {
                    if (bd._count < _basket[i]._count)
                    {
                        MessageBox.Show("Error: " + "количество товара в корзине больше чем существует на складе.\n" +
                            "На складе: " + bd._zapchast + " - " + bd._izgotovitel + " - " + bd._mark + " - " + bd._count + "\n" +
                            "В корзине: " + _basket[i]._zapchast + " - " + _basket[i]._izgotovitel + " - " + _basket[i]._mark + " - " + _basket[i]._count
                            , "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Error: " + "товара не существует на складе.\n" +
                            _basket[i]._zapchast + " - " + _basket[i]._izgotovitel + " - " + _basket[i]._mark + " - " + _basket[i]._count
                            , "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private DataBase CheckExistenceTable(DataBase bd, string table)
        {
            SqlCommand sqlCommand       = new SqlCommand("SELECT * FROM " + table, _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<DataBase> tempTable    = new List<DataBase>();

            while (sqlDataReader.Read()) // Read products table to List<class>
            {
                DataBase bt = new DataBase();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(),
                            sqlDataReader[2].ToString(), sqlDataReader[3].ToString(),
                            (int)sqlDataReader[4], (int)sqlDataReader[5]);
                tempTable.Add(bt);
            }

            for (int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i]._zapchast      == bd._zapchast &&
                    tempTable[i]._mark          == bd._mark &&
                    tempTable[i]._izgotovitel   == bd._izgotovitel)
                {
                    sqlDataReader.Close();
                    return tempTable[i];
                }
            }
            sqlDataReader.Close();
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExecuteAnyQuery("DELETE FROM Basket", _sqlConnection); //clear basket
        }
    }
}
