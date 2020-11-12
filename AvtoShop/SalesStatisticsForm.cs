﻿using System;
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
    public partial class SalesStatisticsForm : Form
    {
        private SqlConnection _sqlConnection = null;
        private SqlCommandBuilder _sqlBuilder = null;
        private SqlDataAdapter _sqlDataAdapter = null;
        private DataSet _dataSet = null;

        public SalesStatisticsForm()
        {
            InitializeComponent();
        }

        private void SalesStatisticsForm_Load(object sender, EventArgs e)
        {
            set_date();
            _sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dimma\Desktop\AvtoShop\AvtoShop\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }

        private void set_date()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy hh:mm:ss";
            dateTimePicker2.CustomFormat = "dd/MM/yyyy hh:mm:ss";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT * FROM SoldStat", _sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init
                _dataSet        = new DataSet(); //init

                _sqlDataAdapter.Fill(_dataSet, "SoldStat"); // заполнение
                dataGridView1.DataSource = _dataSet.Tables["SoldStat"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
