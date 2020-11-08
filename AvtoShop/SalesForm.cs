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
        public class Products
        {
            public int _id;
            public string _zapchast;
            public string _mark;
            public string _izgotovitel;
            public int _price_per_one;
            public int _count;

            public void Add_data(int id, string zap, string mark, string izg, int price, int count)
            {
                _id = id;
                _zapchast = zap;
                _mark = mark;
                _izgotovitel = izg;
                _price_per_one = price;
                _count = count;
            }
        }

        private SqlCommandBuilder _sqlBuilder = null;
        private SqlConnection _sqlConnection = null;
        private SqlCommand _sqlCommand = null;
        private SqlDataAdapter _sqlDataAdapter = null;
        private DataSet _dataSet = null;
        private List<Products> _products = null;

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
            if (countTextBox1.Text == "")
            {
                MessageBox.Show("Добавьте количество товара которое хотите купить.", "Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Products changeTable = new Products();

            if (CheckExistenceTable(ref changeTable)) // существует ли на складе товар
            {
                string queryDB = "";

                if (changeTable._count == int.Parse(countTextBox1.Text))
                {
                    queryDB = "DELETE Products " +
                              "WHERE Id = " + changeTable._id + ";";

                    MessageBox.Show("После покупки, товара - " +
                       izgotovitelBox2.Text + " - " + zapchastBox1.Text + " - " +
                       markBox3.Text + " - " + countTextBox1.Text + "шт., не останется на складе.",
                       "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (changeTable._count > int.Parse(countTextBox1.Text))
                {
                    queryDB = "UPDATE Products SET " +
                              "Количество = '" + (changeTable._count - int.Parse(countTextBox1.Text)) +
                              "' WHERE Id = " + changeTable._id + ";";
                }
                else
                {
                    MessageBox.Show("Такого количества товара нет на складе. \n" +
                       "На складе существует: " + changeTable._izgotovitel + " - " 
                       + changeTable._zapchast + " - " + changeTable._mark + " - " + changeTable._count + "шт.",
                       "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Товар успешно куплен!\n" +
                       izgotovitelBox2.Text + " - " + zapchastBox1.Text + " - " +
                       markBox3.Text + " - " + countTextBox1.Text + "шт.",
                       "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetAllComboBox();
            }
            else // не может быть но мало ли
            {
                MessageBox.Show("Товара не существует, перепроверьте данные. Возможно ошибка программная.",
                    "Предупреждение!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool CheckExistenceTable(ref Products products)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Products", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<Products> tempTable = new List<Products>();

            while (sqlDataReader.Read()) // Read products table to List<class>
            {
                Products bt = new Products();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[4].ToString(), 
                            (int)sqlDataReader[5], (int)sqlDataReader[6]);
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
            _products = new List<Products>(); // update

            while (sqlDataReader.Read())
            {
                Products bt = new Products();

                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), 
                            sqlDataReader[2].ToString(), sqlDataReader[4].ToString(), 
                            (int)sqlDataReader[5], (int)sqlDataReader[6]);

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
            Storage storage = new Storage();
            storage.Show();
        }

        private void soldpriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e){}
        private void label6_Click(object sender, EventArgs e){}
        private void soldpriceTextBox_TextChanged(object sender, EventArgs e){}
    }
}