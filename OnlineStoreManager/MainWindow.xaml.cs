using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace OnlineStoreManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        string sqlConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=True;";
        string sqliteConnectionString = @"Data Source=C:\Users\bv03a\Downloads\Purchases.db;Version=3;Busy Timeout=3000;";

        private void LoadCustomers()
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT ID, LastName, FirstName, MiddleName, PhoneNumber, Email FROM [Customers]";
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();

                    var customers = new List<Customer>();
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            ID = reader.GetInt32(0),
                            LastName = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Email = reader.GetString(5)
                        });
                    }

                    dataGridCustomers.ItemsSource = customers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();  // Закрываем соединение после выполнения
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
                finally
                {
                    sqliteConnection.Close();  // Закрываем соединение после выполнения
                }
            }
        }

        private void dataGridCustomers_Sorting(object sender, DataGridSortingEventArgs e)
        {
            LoadCustomers();  // Обновляем список клиентов при сортировке
        }

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();  // Обновляем клиентов
            dataGridPurchases.ItemsSource = null;  // Очищаем нижний DataGrid
        }

        private void dataGridCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                string email = selectedCustomer.Email;
                LoadPurchases(email);  // Загружаем покупки для выбранного клиента
            }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow addCustomerWindow = new AddCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
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
                    finally
                    {
                        sqlConnection.Close();  // Закрываем соединение после выполнения
                    }
                }
            }
        }

        private void btnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)dataGridCustomers.SelectedItem;
                string email = selectedCustomer.Email;

                AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
                if (addPurchaseWindow.ShowDialog() == true)
                {
                    int productCode = addPurchaseWindow.ProductCode;
                    string productName = addPurchaseWindow.ProductName;

                    using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
                    {
                        sqliteConnection.Open();
                        using (var transaction = sqliteConnection.BeginTransaction())
                        {
                            try
                            {
                                string query = "INSERT INTO Purchases (Email, ProductCode, ProductName) VALUES (@Email, @ProductCode, @ProductName)";
                                SQLiteCommand command = new SQLiteCommand(query, sqliteConnection);
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@ProductCode", productCode);
                                command.Parameters.AddWithValue("@ProductName", productName);
                                command.ExecuteNonQuery();

                                transaction.Commit();  // Коммит транзакции
                                MessageBox.Show("Покупка успешно добавлена.");
                                LoadPurchases(email);  // Обновляем список покупок
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();  // Откат транзакции при ошибке
                                MessageBox.Show("Ошибка добавления покупки: " + ex.Message);
                            }
                            finally
                            {
                                sqliteConnection.Close();  // Закрываем соединение после выполнения
                            }
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
                Customer selectedCustomer = (Customer)dataGridCustomers.SelectedItem;

                int id = selectedCustomer.ID;
                string lastName = selectedCustomer.LastName;
                string firstName = selectedCustomer.FirstName;
                string middleName = selectedCustomer.MiddleName;
                string phoneNumber = selectedCustomer.PhoneNumber;
                string email = selectedCustomer.Email;

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
                        finally
                        {
                            sqlConnection.Close();  // Закрываем соединение после выполнения
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
                    finally
                    {
                        sqliteConnection.Close();  // Закрываем соединение после выполнения
                    }
                }

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
                    finally
                    {
                        sqlConnection.Close();  // Закрываем соединение после выполнения
                    }
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem != null)
            {
                Customer selectedCustomer = (Customer)dataGridCustomers.SelectedItem;

                int id = selectedCustomer.ID;

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
                            LoadCustomers();  // Обновляем список клиентов после удаления
                            dataGridPurchases.ItemsSource = null;  // Очистить покупки после удаления клиента
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка удаления клиента: " + ex.Message);
                        }
                        finally
                        {
                            sqlConnection.Close();  // Закрываем соединение после выполнения
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
