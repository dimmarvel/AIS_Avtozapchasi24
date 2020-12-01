using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class PurchaseForm : Form
    {

        private SqlConnection _sqlConnection    = null;
        private List<DataBase> _buyTable        = null;
        private SqlCommand _sqlCommand          = null;

        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            try
            {
                _sqlConnection = new SqlConnection(Constants._connectStr);
                _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)

                _buyTable = new List<DataBase>();
                ReadDataToClass();

                izgotovitelBox2.Text = _buyTable[0]._izgotovitel;
                SetAllComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message , "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SetAllComboBox()
        {
            izgotovitelBox2.Items.Clear();
            List<string> _izgotovitelComboBox = GetUniqIzgotovitel();

            foreach (var it in _izgotovitelComboBox)
                izgotovitelBox2.Items.Add(it);

            SetZapchastComboBox(izgotovitelBox2.Text);
            SetMarkComboBox(zapchastBox1.Text, izgotovitelBox2.Text);
        }

        private List<string> GetUniqIzgotovitel()
        {
            List<string> temp = new List<string>();
            foreach (var it in _buyTable)
            {
                if (temp.Contains(it._izgotovitel)) continue;
                temp.Add(it._izgotovitel);
            }
            return temp;
        }

        private void SetZapchastComboBox(string izgotovitel)
        {
            zapchastBox1.Items.Clear();

            foreach (var it in _buyTable)
                if (izgotovitel == it._izgotovitel)
                    if (!zapchastBox1.Items.Contains(it._zapchast))
                        zapchastBox1.Items.Add(it._zapchast);
            zapchastBox1.Text = zapchastBox1.Items[0].ToString();
        }

        private void SetMarkComboBox(string zapchast, string izgotovitel)
        {
            markBox3.Items.Clear();

            foreach (var it in _buyTable)
                if (zapchast == it._zapchast && izgotovitel == it._izgotovitel)
                    if (!markBox3.Items.Contains(it._mark))
                        markBox3.Items.Add(it._mark);
            markBox3.Text = markBox3.Items[0].ToString();
            RecalcPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (countTextBox1.Text == "")
            {
                MessageBox.Show("Добавьте количество товара которое хотите купить.", "Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (soldpriceTextBox.Text == "")
            {
                MessageBox.Show("Цена продажи не указана.", "Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            DataBase changeTable = new DataBase();

            if (CheckExistenceTable(ref changeTable)) // существует ли на складе товар
            {
                //_price_per_one здесь в роли количества если юзаем таблицу Products
                string queryDB = "UPDATE Products SET " +
                                "Количество = '" + (changeTable._count + int.Parse(countTextBox1.Text)) +
                                "', Цена = '" + soldpriceTextBox.Text +
                                "' WHERE Id = " + changeTable._id + ";";

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Товар успешно куплен!\n" +
                   izgotovitelBox2.Text + " - " + zapchastBox1.Text + " - " +
                   markBox3.Text + " - " + countTextBox1.Text + "шт.",
                   "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                string queryDB = "INSERT INTO[Products](Запчасть, Марка, Изготовитель, Цена, Количество)" +
                    "VALUES(@zapchast, @mark, @izgotovitel, @price, @count)";

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.Parameters.AddWithValue("zapchast", zapchastBox1.Text);
                _sqlCommand.Parameters.AddWithValue("mark", markBox3.Text);
                _sqlCommand.Parameters.AddWithValue("izgotovitel", izgotovitelBox2.Text);
                _sqlCommand.Parameters.AddWithValue("price", soldpriceTextBox.Text);
                _sqlCommand.Parameters.AddWithValue("count", countTextBox1.Text);

                _sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Товар успешно куплен!","Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckExistenceTable(ref DataBase buyTable)
        {
            SqlCommand sqlCommand       = new SqlCommand("SELECT * FROM Products", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<DataBase> tempTable    = new List<DataBase>();

            while (sqlDataReader.Read()) // Read products table to List<class>
            {
                DataBase bt = new DataBase();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), sqlDataReader[2].ToString(),
                                        sqlDataReader[3].ToString(), (int)sqlDataReader[4], (int)sqlDataReader[5]);
                tempTable.Add(bt);
            }

            for(int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i]._zapchast      == zapchastBox1.Text &&
                    tempTable[i]._mark          == markBox3.Text &&
                    tempTable[i]._izgotovitel   == izgotovitelBox2.Text)
                {
                    sqlDataReader.Close();
                    buyTable = tempTable[i];
                    return true;
                }
            }
            sqlDataReader.Close();
            return false;

        }
        private void ReadDataToClass()
        {
            SqlCommand sqlCommand       = new SqlCommand("SELECT * FROM BuyTable", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                DataBase bt = new DataBase();

                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[3].ToString(), 
                            (int)sqlDataReader[4], 0);

                _buyTable.Add(bt);
            }

            sqlDataReader.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMarkComboBox(zapchastBox1.Text,izgotovitelBox2.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetZapchastComboBox(izgotovitelBox2.Text);
            SetMarkComboBox(zapchastBox1.Text, izgotovitelBox2.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(int.Parse(countTextBox1.Text) < 100001)
            {
                RecalcPrice();
            }else
            {
                MessageBox.Show("Слишком большое количество.", "Error!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                countTextBox1.Text = "1";
            }
        }

        public void RecalcPrice()
        {
            priceLabel.Text = "";
            for (int i = 0; i < _buyTable.Count; i++) // find price
            {
                if(_buyTable[i]._izgotovitel == izgotovitelBox2.Text && 
                   _buyTable[i]._zapchast == zapchastBox1.Text && 
                   _buyTable[i]._mark == markBox3.Text)
                {
                    if(countTextBox1.Text == "")
                        priceLabel.Text += "0$ (за 1 шт. = " +_buyTable[i]._price_per_one + "$)";
                    else
                        priceLabel.Text += int.Parse(countTextBox1.Text) * _buyTable[i]._price_per_one + "$ (за 1 шт. = "
                            + _buyTable[i]._price_per_one + "$)";
                    break;
                }
            }

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Postavshiki postavshiki = new Postavshiki();
            postavshiki.Show();
        }

        private void soldpriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void soldpriceTextBox_TextChanged_1(object sender, EventArgs e)
        {
            if (int.Parse(soldpriceTextBox.Text) > 10000001)
            {
                MessageBox.Show("Слишком большая цена продажи.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                countTextBox1.Text = "1";
            }
        }

        private void izgotovitelBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void zapchastBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void markBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void markBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void PurchaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _sqlConnection.Close();
        }
    }
}
