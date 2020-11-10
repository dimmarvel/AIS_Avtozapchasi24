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

        private SqlConnection _sqlConnection = null;
        private SqlCommand _sqlCommand = null;
        private List<DataBase> _products = null;
        private Storage storage = null;
        private Basket basket = null;
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
                
                ReadDataToClass();
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
            izgotovitelBox2.Text = _products[0]._izgotovitel;
            izgotovitelBox2.Items.Clear();
            ReadDataToClass();//Update data in class 
            List<string> _izgotovitelComboBox = GetUniqIzgotovitel();

            foreach (var it in _izgotovitelComboBox)
                izgotovitelBox2.Items.Add(it);

            SetZapchastComboBox(izgotovitelBox2.Text);
            SetMarkComboBox(zapchastBox1.Text, izgotovitelBox2.Text);
        }

        private List<string> GetUniqIzgotovitel()
        {
            List<string> temp = new List<string>();
            foreach (var it in _products)
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

            foreach (var it in _products)
                if (izgotovitel == it._izgotovitel)
                    if (!zapchastBox1.Items.Contains(it._zapchast))
                        zapchastBox1.Items.Add(it._zapchast);
            zapchastBox1.Text = zapchastBox1.Items[0].ToString();
        }

        private void SetMarkComboBox(string zapchast, string izgotovitel)
        {
            markBox3.Items.Clear();

            foreach (var it in _products)
                if (zapchast == it._zapchast && izgotovitel == it._izgotovitel)
                    if (!markBox3.Items.Contains(it._mark))
                        markBox3.Items.Add(it._mark);
            markBox3.Text = markBox3.Items[0].ToString();
            RecalcPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (countTextBox1.Text == "")
                {
                    MessageBox.Show("Добавьте количество товара которое хотите купить.", "Предупреждение!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataBase changeTable = new DataBase();
                
                if (CheckExistenceTable(ref changeTable, "Products")) // существует ли на складе товар
                {
                    string queryDB = "INSERT INTO Basket (Запчасть, Марка, Изготовитель, Цена, Количество)" + " VALUES(N'" +
                                    changeTable._zapchast + "', N'" +
                                    changeTable._mark + "', N'" +
                                    changeTable._izgotovitel + "', '" +
                                    changeTable._price_per_one + "', '" +
                                    countTextBox1.Text + "');";

                    DataBase basketdata = new DataBase();

                    if (CheckExistenceTable(ref basketdata, "Basket")) //если в корзине уже есть такой товар
                    {
                        int count = basketdata._count + int.Parse(countTextBox1.Text); // сколько в корзине + сколько добавить ещё
                        queryDB = "UPDATE Basket SET " + "Количество = '" + count +
                                  "' WHERE Id = " + basketdata._id + ";";
                        // ТУТ ОСТАНОВИЛСЯ СДЕЛАЛ ДОБАВЛЕНИЕ В КОРЗИНУ

                        if (changeTable._count == count)
                        {
                            MessageBox.Show("После покупки, товара - " + izgotovitelBox2.Text + " - " + zapchastBox1.Text +
                                            " - " + markBox3.Text + " - " + countTextBox1.Text + "шт., не останется на складе.",
                                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (changeTable._count < count)
                        {
                            MessageBox.Show("Такого количества товара нет на складе. \n" + "На складе существует: " 
                                            + changeTable._izgotovitel + " - " + changeTable._zapchast + " - " 
                                            + changeTable._mark + " - " + changeTable._count + "шт.",
                                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                    _sqlCommand.ExecuteNonQuery();

                    MessageBox.Show("Товар добавлен в корзину!\n" +
                           izgotovitelBox2.Text + " - " + zapchastBox1.Text + " - " +
                           markBox3.Text + " - " + countTextBox1.Text + "шт.",
                           "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetAllComboBox();
                }
                else
                {
                    MessageBox.Show("Товара не существует, перепроверьте данные. Возможно ошибка программная.",
                        "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //queryDB = "DELETE Products " +
            //"WHERE Id = " + changeTable._id + ";";
            //queryDB = "UPDATE Products SET " +
            //          "Количество = '" + (changeTable._count - int.Parse(countTextBox1.Text)) +
            //          "' WHERE Id = " + changeTable._id + ";";
        }

        private bool CheckExistenceTable(ref DataBase products, string table)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM " + table, _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<DataBase> tempTable = new List<DataBase>();

            while (sqlDataReader.Read()) // Read products table to List<class>
            {
                DataBase bt = new DataBase();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[3].ToString(), 
                            (int)sqlDataReader[4], (int)sqlDataReader[5]);
                tempTable.Add(bt);
            }

            for(int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i]._zapchast == zapchastBox1.Text &&
                    tempTable[i]._mark == markBox3.Text &&
                    tempTable[i]._izgotovitel == izgotovitelBox2.Text)
                {
                    sqlDataReader.Close();
                    products = tempTable[i];
                    return true;
                }
            }
            sqlDataReader.Close();
            return false;
        }

        private void ReadDataToClass()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Products", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            _products = new List<DataBase>(); // update

            while (sqlDataReader.Read())
            {
                DataBase bt = new DataBase();

                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[3].ToString(), 
                            (int)sqlDataReader[4], (int)sqlDataReader[5]);

                _products.Add(bt);
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
            }
            else
            {
                MessageBox.Show("Слишком большое количество заказанных деталей.", "Error!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                countTextBox1.Text = "1";
            }
        }

        public void RecalcPrice()
        {
            priceLabel.Text = "";
            for (int i = 0; i < _products.Count; i++) // find price
            {
                if(_products[i]._izgotovitel == izgotovitelBox2.Text && 
                   _products[i]._zapchast == zapchastBox1.Text && 
                   _products[i]._mark == markBox3.Text)
                {
                    if(countTextBox1.Text == "")
                        priceLabel.Text += "0$ (за 1 шт. = " +_products[i]._price_per_one + "$)";
                    else
                        priceLabel.Text += int.Parse(countTextBox1.Text) * _products[i]._price_per_one + "$ (за 1 шт. = "
                            + _products[i]._price_per_one + "$)";
                    break;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                storage = new Storage();
                storage.Show();
        }

        private void soldpriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
                basket = new Basket();
                basket.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){}
        private void label6_Click(object sender, EventArgs e){}
        private void soldpriceTextBox_TextChanged(object sender, EventArgs e){}

    }
}