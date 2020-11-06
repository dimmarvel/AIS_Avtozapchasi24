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
    //public class BuyTable
    //{
    //    public List<string> _zapchast;
    //    public List<string> _mark;
    //    public List<string> _izgotovitel;
    //    public List<int> _price_per_one;
    //    public BuyTable()
    //    {
    //        _zapchast = new List<string>();
    //        _mark = new List<string>();
    //        _izgotovitel = new List<string>();
    //        _price_per_one = new List<int>();
    //    }
    //    public void Add_data(string _zap, string _mar, string _izg, int _price)
    //    {
    //        _zapchast.Add(_zap);
    //        _mark.Add(_mar);
    //        _izgotovitel.Add(_izg);
    //        _price_per_one.Add(_price);
    //    }
    //    public void DeleteDublicate()
    //    {
    //        _zapchast =_zapchast.Distinct().ToList();
    //    }
    //}

    public partial class PurchaseForm : Form
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

        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dimma\Desktop\AvtoShop\AvtoShop\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            _buyTable = new List<BuyTable>();
            ReadDataToClass();

            comboBox2.Text = _buyTable[0]._izgotovitel;
            SetAllComboBox();
        }

        private void SetAllComboBox()
        {
            comboBox2.Items.Clear();
            List<string> _izgotovitelComboBox = GetUniqIzgotovitel();

            foreach (var it in _izgotovitelComboBox)
                comboBox2.Items.Add(it);

            SetZapchastComboBox(comboBox2.Text);
            SetMarkComboBox(comboBox1.Text, comboBox2.Text);
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
            comboBox1.Items.Clear();

            foreach (var it in _buyTable)
                if (izgotovitel == it._izgotovitel)
                    if (!comboBox1.Items.Contains(it._zapchast))
                        comboBox1.Items.Add(it._zapchast);
            comboBox1.Text = comboBox1.Items[0].ToString();
        }
        private void SetMarkComboBox(string zapchast, string izgotovitel)
        {
            comboBox3.Items.Clear();

            foreach (var it in _buyTable)
                if (zapchast == it._zapchast && izgotovitel == it._izgotovitel)
                    if (!comboBox3.Items.Contains(it._mark))
                        comboBox3.Items.Add(it._mark);
            comboBox3.Text = comboBox3.Items[0].ToString();
            RecalcPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Добавьте количество товара которое хотите купить.", "Предупреждение!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CheckExistenceTable()) // существует ли на складе товар
            {
                MessageBox.Show("А такое в таблице ЕСТЬ");
            }
            else
            {
                string queryDB = "INSERT INTO[Products](Запчасть, Марка, Наличие, Изготовитель, Цена, Количество)" +
                    "VALUES(@zapchast, @mark, @nalichie, @izgotovitel, @price, @count)";

                _sqlCommand = new SqlCommand(queryDB, _sqlConnection);
                _sqlCommand.Parameters.AddWithValue("zapchast", comboBox1.Text);
                _sqlCommand.Parameters.AddWithValue("mark", comboBox3.Text);
                _sqlCommand.Parameters.AddWithValue("nalichie", "True");
                _sqlCommand.Parameters.AddWithValue("izgotovitel", comboBox2.Text);
                _sqlCommand.Parameters.AddWithValue("price", "0");
                _sqlCommand.Parameters.AddWithValue("count", textBox1.Text);
                _sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Товар успешно куплен!\nТовара (" + comboBox2.Text + " " + comboBox1.Text + " " + comboBox3.Text +
                    ") ещё небыло на складе. Пожалуйста, установите цену этому товару в разделе \"Товары\" по умолчанию цена 0$.");
                // добавить в таблицу product если не существует такой записи по данным из комбобоксов
            }

        }
        private bool CheckExistenceTable()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Products", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<BuyTable> tempTable = new List<BuyTable>();
            while (sqlDataReader.Read())
            {
                BuyTable bt = new BuyTable();
                bt.Add_data((int)sqlDataReader[0], sqlDataReader[1].ToString(), sqlDataReader[2].ToString(),
                                        sqlDataReader[4].ToString(), (int)sqlDataReader[5]);
                tempTable.Add(bt);
            }

            for(int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i]._zapchast == comboBox1.Text &&
                    tempTable[i]._mark == comboBox3.Text &&
                    tempTable[i]._izgotovitel == comboBox2.Text)
                {
                    sqlDataReader.Close();
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
            SetMarkComboBox(comboBox1.Text,comboBox2.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetZapchastComboBox(comboBox2.Text);
            SetMarkComboBox(comboBox1.Text, comboBox2.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(int.Parse(textBox1.Text) < 100001)
            {
                RecalcPrice();
            }else
            {
                MessageBox.Show("Слишком большое количество заказанных деталей.", "Error!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "1";
            }
        }

        public void RecalcPrice()
        {
            label6.Text = "";
            for (int i = 0; i < _buyTable.Count; i++) // find price
            {
                if(_buyTable[i]._izgotovitel == comboBox2.Text && 
                   _buyTable[i]._zapchast == comboBox1.Text && 
                   _buyTable[i]._mark == comboBox3.Text)
                {
                    if(textBox1.Text == "")
                        label6.Text += "0$ (за 1 шт. = " +_buyTable[i]._price_per_one + "$)";
                    else
                        label6.Text += int.Parse(textBox1.Text) * _buyTable[i]._price_per_one + "$ (за 1 шт. = "
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
    }
}
