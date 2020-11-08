using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvtoShop
{

    public partial class SalesForm : Form
    {
        public class BuyTable
        {
            public int _id;
            public string _zapchast;
            public string _mark;
            public string _izgotovitel;
            public int _price_per_one;
            public void Add_data(int id, string zap, string mark, string izg, int price)
            {
                _id = id;
                _zapchast = zap;
                _mark = mark;
                _izgotovitel = izg;
                _price_per_one = price;
            }
        }

        private SqlCommandBuilder _sqlBuilder = null;
        private SqlConnection _sqlConnection = null;
        private SqlCommand _sqlCommand = null;
        private SqlDataAdapter _sqlDataAdapter = null;
        private DataSet _dataSet = null;
        private List<BuyTable> _buyTable = null;

        public SalesForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            try
            {

                _sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dimma\Desktop\AvtoShop\AvtoShop\Database1.mdf;Integrated Security=True");
                _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
                _buyTable = new List<BuyTable>();
                ReadDataToClass();

                izgotovitelBox2.Text = _buyTable[0]._izgotovitel;
                SetAllComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message , "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (temp.Contains(it._izgotovitel))
                    continue;
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
                MessageBox.Show("Добавьте цену продажи.", "Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuyTable changeTable = new BuyTable();

            if (CheckExistenceTable(ref changeTable)) // существует ли на складе товар
            { 
                //_price_per_one здесь в роли количества если юзаем таблицу Products
                string queryDB = "UPDATE Products SET " +
                                "Количество = '"+ (changeTable._price_per_one + int.Parse(countTextBox1.Text)) +
                                "', Цена = '" + soldpriceTextBox.Text + 
                                "' WHERE Id = " + changeTable._id + ";";

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Товар успешно куплен и добавлен на склад!\n" +
                   izgotovitelBox2.Text + " - " + zapchastBox1.Text + " - " + 
                   markBox3.Text + " - " + countTextBox1.Text + "шт.", 
                   "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                string queryDB = "INSERT INTO[Products](Запчасть, Марка, Наличие, Изготовитель, Цена, Количество)" +
                    "VALUES(@zapchast, @mark, @nalichie, @izgotovitel, @price, @count)";

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.Parameters.AddWithValue("zapchast", zapchastBox1.Text);
                _sqlCommand.Parameters.AddWithValue("mark", markBox3.Text);
                _sqlCommand.Parameters.AddWithValue("nalichie", "True");
                _sqlCommand.Parameters.AddWithValue("izgotovitel", izgotovitelBox2.Text);
                _sqlCommand.Parameters.AddWithValue("price", soldpriceTextBox.Text);
                _sqlCommand.Parameters.AddWithValue("count", countTextBox1.Text);
                
                _sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Товар успешно куплен!\nТовара (" + izgotovitelBox2.Text + " " + zapchastBox1.Text + " " + markBox3.Text +
                    ") ещё небыло на складе. Пожалуйста, установите цену этому товару в разделе \"Товары\" по умолчанию цена 0$.",
                    "Предупреждение!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // добавить в таблицу product если не существует такой записи по данным из комбобоксов
            }

        }
        private bool CheckExistenceTable(ref BuyTable buyTable)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Products", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<BuyTable> tempTable = new List<BuyTable>();

            while (sqlDataReader.Read()) // Read products table to List<class>
            {
                BuyTable bt = new BuyTable();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), sqlDataReader[2].ToString(),
                                        sqlDataReader[4].ToString(), (int)sqlDataReader[6]);
                tempTable.Add(bt);
            }

            for(int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i]._zapchast == zapchastBox1.Text &&
                    tempTable[i]._mark == markBox3.Text &&
                    tempTable[i]._izgotovitel == izgotovitelBox2.Text)
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
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM BuyTable", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                BuyTable bt = new BuyTable();

                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[3].ToString(), 
                            (int)sqlDataReader[4]);

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
                MessageBox.Show("Слишком большое количество заказанных деталей.", "Error!", 
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
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void soldpriceTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void soldpriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
