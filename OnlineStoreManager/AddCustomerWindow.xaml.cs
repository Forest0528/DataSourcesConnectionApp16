using System.Windows;

namespace OnlineStoreManager
{
    public partial class AddCustomerWindow : Window
    {
        // Свойства для хранения данных, введённых пользователем
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Сохранить"
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно добавить проверку, что все обязательные поля заполнены
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (Фамилия, Имя и Email).");
                return;
            }

            // Присваиваем значения свойствам из текстовых полей
            LastName = txtLastName.Text;
            FirstName = txtFirstName.Text;
            MiddleName = txtMiddleName.Text;
            PhoneNumber = txtPhoneNumber.Text;
            Email = txtEmail.Text;

            // Устанавливаем положительный результат диалога и закрываем окно
            DialogResult = true;
            Close();
        }

        // Обработчик для кнопки "Отмена"
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно, не сохраняя данные
            DialogResult = false;
            Close();
        }
    }
}
