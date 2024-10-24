using System.Windows;

namespace OnlineStoreManager
{
    public partial class AddPurchaseWindow : Window
    {
        public int ProductCode { get; private set; }
        public string ProductName { get; private set; }

        public AddPurchaseWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductCode.Text) ||
                string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!int.TryParse(txtProductCode.Text, out int code))
            {
                MessageBox.Show("Код товара должен быть числом.");
                return;
            }

            ProductCode = code;
            ProductName = txtProductName.Text;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
