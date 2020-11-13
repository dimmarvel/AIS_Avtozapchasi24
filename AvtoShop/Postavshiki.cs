﻿using System;
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

        public Postavshiki()
        {
            InitializeComponent();
        }

        private void Postavshiki_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\Database1.mdf;Integrated Security=True");
            _sqlConnection.Open(); //connect to database (load data from bd in datagrid view)
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // SELECT(выбрать все сущности из таблицы) остальное для управления по таблице
                _sqlDataAdapter = new SqlDataAdapter("SELECT * FROM BuyTable", _sqlConnection);
                _sqlBuilder     = new SqlCommandBuilder(_sqlDataAdapter); //init
                _dataSet        = new DataSet(); //init

                _sqlDataAdapter.Fill(_dataSet, "BuyTable"); // заполнение
                dataGridView1.DataSource = _dataSet.Tables["BuyTable"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
