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
            public string _zapchast;
            public string _mark;
            public string _izgotovitel;
            public int _price_per_one;
            public void Add_data(string _zap, string _mar, string _izg, int _price)
            {
                _zapchast = _zap;
                _mark = _mar;
                _izgotovitel = _izg;
                _price_per_one = _price;
            }
        }

        private SqlConnection _sqlConnection = null;
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
            SetMarkComboBox(comboBox1.Text);
        }

        private List<string> GetUniqIzgotovitel()
        {
            List<string> temp = new List<string>();
            foreach (var it in _buyTable)
            {
                if(temp.Contains(it._izgotovitel))
                    continue;
                temp.Add(it._izgotovitel);
            }
            return temp;
        }
        private void SetZapchastComboBox(string izgotovitel)
        {
            comboBox1.Items.Clear();

            foreach (var it in _buyTable)
                if(izgotovitel == it._izgotovitel)
                    if (!comboBox1.Items.Contains(it._zapchast))
                        comboBox1.Items.Add(it._zapchast);
            comboBox1.Text = comboBox1.Items[0].ToString();
        }
        private void SetMarkComboBox(string zapchast)
        {
            comboBox3.Items.Clear();

            foreach (var it in _buyTable)
                if (zapchast == it._zapchast)
                    if (!comboBox3.Items.Contains(it._mark))
                        comboBox3.Items.Add(it._mark);
            comboBox3.Text = comboBox3.Items[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = "";
            foreach(var it in _buyTable)
            { 
               temp += it._zapchast + " - " +
                           it._mark + " - " +
                           it._izgotovitel + " - " +
                           it._price_per_one + "\n";
            }
            MessageBox.Show(temp);

        }
        private void ReadDataToClass()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM BuyTable", _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                BuyTable bt = new BuyTable();
                bt.Add_data(sqlDataReader[1].ToString(), sqlDataReader[2].ToString(),
                                        sqlDataReader[3].ToString(), (int)sqlDataReader[4]);
                _buyTable.Add(bt);
            }
            sqlDataReader.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMarkComboBox(comboBox1.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetZapchastComboBox(comboBox2.Text);
            SetMarkComboBox(comboBox1.Text);
        }
    }
}
