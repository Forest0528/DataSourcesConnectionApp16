using System;
using System.Data;
using System.Data.SqlClient;  // Для MSSQLLocalDB
using System.Data.SQLite; // Для SQLite
using System.Windows;
using System.Windows.Controls; // Добавлено пространство имен




namespace OnlineStoreManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCustomers(); // Добавьте этот вызов
        }

        string sqlConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=True;";

        string sqliteConnectionString = @"Data Source=|DataDirectory|\Purchases.sqlite;Version=3;";


        private void LoadCustomers()
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Customers", sqlConnection);
                    DataTable customersTable = new DataTable();
                    adapter.Fill(customersTable);
                    dataGridCustomers.ItemsSource = customersTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
                }
            }
        }

        private void LoadPurchases(string email)
        {
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
            {
                try
                {
                    sqliteConnection.Open();
                    string query = "SELECT * FROM Purchases WHERE Email = @Email";
                    SQLiteCommand command = new SQLiteCommand(query, sqliteConnection);
                    command.Parameters.AddWithValue("@Email", email);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable purchasesTable = new DataTable();
                    adapter.Fill(purchasesTable);
                    dataGridPurchases.ItemsSource = purchasesTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки покупок: " + ex.Message);
                }
            }
        }

        private void dataGridCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridCustomers.SelectedItem;
                string email = row["Email"].ToString();
                LoadPurchases(email);
            }
        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Откройте окно для ввода данных клиента или используйте поля ввода на форме
            AddCustomerWindow addCustomerWindow = new AddCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
                // Получите данные из окна
                string lastName = addCustomerWindow.LastName;
                string firstName = addCustomerWindow.FirstName;
                string middleName = addCustomerWindow.MiddleName;
                string phoneNumber = addCustomerWindow.PhoneNumber;
                string email = addCustomerWindow.Email;

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        string query = "INSERT INTO Customers (LastName, FirstName, MiddleName, PhoneNumber, Email) VALUES (@LastName, @FirstName, @MiddleName, @PhoneNumber, @Email)";
                        SqlCommand command = new SqlCommand(query, sqlConnection);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@MiddleName", middleName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Email", email);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Клиент успешно добавлен.");
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления клиента: " + ex.Message);
                    }
                }
            }
        }

        private void btnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                DataRowView customerRow = (DataRowView)dataGridCustomers.SelectedItem;
                string email = customerRow["Email"].ToString();

                // Откройте окно для ввода данных покупки
                AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
                if (addPurchaseWindow.ShowDialog() == true)
                {
                    int productCode = addPurchaseWindow.ProductCode;
                    string productName = addPurchaseWindow.ProductName;

                    using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
                    {
                        try
                        {
                            sqliteConnection.Open();
                            string query = "INSERT INTO Purchases (Email, ProductCode, ProductName) VALUES (@Email, @ProductCode, @ProductName)";
                            SQLiteCommand command = new SQLiteCommand(query, sqliteConnection);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@ProductCode", productCode);
                            command.Parameters.AddWithValue("@ProductName", productName);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Покупка успешно добавлена.");
                            LoadPurchases(email);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка добавления покупки: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента.");
            }
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridCustomers.SelectedItem;
                int id = Convert.ToInt32(row["ID"]);
                string lastName = row["LastName"].ToString();
                string firstName = row["FirstName"].ToString();
                string middleName = row["MiddleName"].ToString();
                string phoneNumber = row["PhoneNumber"].ToString();
                string email = row["Email"].ToString();



                // Откройте окно для редактирования данных клиента
                EditCustomerWindow editCustomerWindow = new EditCustomerWindow(lastName, firstName, middleName, phoneNumber, email);
                if (editCustomerWindow.ShowDialog() == true)
                {
                    lastName = editCustomerWindow.LastName;
                    firstName = editCustomerWindow.FirstName;
                    middleName = editCustomerWindow.MiddleName;
                    phoneNumber = editCustomerWindow.PhoneNumber;
                    email = editCustomerWindow.Email;

                    using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                    {
                        try
                        {
                            sqlConnection.Open();
                            string query = "UPDATE Customers SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, PhoneNumber = @PhoneNumber, Email = @Email WHERE ID = @ID";
                            SqlCommand command = new SqlCommand(query, sqlConnection);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@MiddleName", middleName);
                            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@ID", id);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Данные клиента обновлены.");
                            LoadCustomers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка обновления клиента: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента.");
            }
        }

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить все данные?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                // Удаляем данные из таблицы Purchases
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
                {
                    try
                    {
                        sqliteConnection.Open();
                        string query = "DELETE FROM Purchases";
                        SQLiteCommand command = new SQLiteCommand(query, sqliteConnection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка очистки данных в SQLite: " + ex.Message);
                    }
                }

                // Удаляем данные из таблицы Customers
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        string query = "DELETE FROM Customers";
                        SqlCommand command = new SqlCommand(query, sqlConnection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Все данные удалены.");
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка очистки данных в MSSQLLocalDB: " + ex.Message);
                    }
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGridCustomers.SelectedItem;
                int id = Convert.ToInt32(row["ID"]);

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                    {
                        try
                        {
                            sqlConnection.Open();
                            string query = "DELETE FROM Customers WHERE ID = @ID";
                            SqlCommand command = new SqlCommand(query, sqlConnection);
                            command.Parameters.AddWithValue("@ID", id);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Клиент успешно удален.");
                            LoadCustomers();
                            dataGridPurchases.ItemsSource = null; // Очистить покупки
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка удаления клиента: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.");
            }
        }




    }
}
