using System;
using System.Windows.Forms;

namespace AvtoShop
{
    public partial class MainMenu : Form
    {
        private static StorageForm          _storageForm;
        private static PurchaseForm         _purchaseForm;
        private static ProductsForm         _productsForm;
        private static SalesStatisticsForm  _salesStatForm;
        private static SalesForm            _salesForm;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e){}

        private void PurchaseButton_Click(object sender, EventArgs e)
        {
            try
            {
                _purchaseForm = new PurchaseForm();
                _purchaseForm.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StorageButton_Click(object sender, EventArgs e)
        {
            try
            {
                _storageForm = new StorageForm();
                _storageForm.Show();
            }
            catch (Exception)
            {
                throw;
            }
}

        private void ProductsButton_Click(object sender, EventArgs e)
        {
            try
            {
                _productsForm = new ProductsForm();
                _productsForm.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SalesStatisticsButton_Click(object sender, EventArgs e)
        {
            try
            {
                _salesStatForm = new SalesStatisticsForm();
                _salesStatForm.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SalesButton_Click(object sender, EventArgs e)
        {
            try
            {
                _salesForm = new SalesForm();
                _salesForm.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
