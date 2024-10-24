using System.Windows;

namespace OnlineStoreManager
{
    public partial class AddCustomerWindow : Window
    {
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            LastName = txtLastName.Text;
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            PhoneNumber = txtPhoneNumber.Text;
            Email = txtEmail.Text;

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
