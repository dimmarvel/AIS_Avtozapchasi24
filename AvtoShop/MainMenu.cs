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
            _purchaseForm = new PurchaseForm();
            _purchaseForm.Show();
        }

        private void StorageButton_Click(object sender, EventArgs e)
        {
            _storageForm = new StorageForm();
            _storageForm.Show();
        }

        private void ProductsButton_Click(object sender, EventArgs e)
        {
            _productsForm = new ProductsForm();
            _productsForm.Show();
        }

        private void SalesStatisticsButton_Click(object sender, EventArgs e)
        {
            _salesStatForm = new SalesStatisticsForm();
            _salesStatForm.Show();
        }

        private void SalesButton_Click(object sender, EventArgs e)
        {
            _salesForm = new SalesForm();
            _salesForm.Show();
        }
    }
}
