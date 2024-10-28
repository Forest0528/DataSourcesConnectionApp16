using Microsoft.EntityFrameworkCore;
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
        private readonly string sqlConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=True;";
        private readonly string sqliteConnectionString = @"Data Source=C:\Users\bv03a\Downloads\Purchases.db;Version=3;Busy Timeout=3000;";
        private readonly ICustomerRepository _customerRepository;
        public MainWindow() : this(new CustomerRepository(new OnlineStoreContext(new DbContextOptions<OnlineStoreContext>())))
        {
        }

        public MainWindow(ICustomerRepository customerRepository)
        {
            InitializeComponent();
            _customerRepository = customerRepository;
            LoadCustomers();
        }


        // Универсальный метод для выполнения SQL-запросов
        private void ExecuteSql(Action<SqlConnection> action)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    action(sqlConnection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        // Универсальный метод для выполнения SQLite-запросов
        private void ExecuteSQLite(Action<SQLiteConnection> action)
        {
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
            {
                try
                {
                    sqliteConnection.Open();
                    action(sqliteConnection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void LoadCustomers()
        {
            dataGridCustomers.ItemsSource = _customerRepository.GetAllCustomers();
        }

        //private void LoadCustomers()
        //{
        //    ExecuteSql(sqlConnection =>
        //    {
        //        string query = "SELECT ID, LastName, FirstName, MiddleName, PhoneNumber, Email FROM [Customers]";
        //        SqlCommand command = new SqlCommand(query, sqlConnection);
        //        SqlDataReader reader = command.ExecuteReader();

        //        var customers = new List<Customer>();
        //        while (reader.Read())
        //        {
        //            customers.Add(new Customer
        //            {
        //                ID = reader.GetInt32(0),
        //                LastName = reader.GetString(1),
        //                FirstName = reader.GetString(2),
        //                MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
        //                PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
        //                Email = reader.GetString(5)
        //            });
        //        }

        //        dataGridCustomers.ItemsSource = customers;
        //    });
        //}

        private void LoadPurchases(string email)
        {
            ExecuteSQLite(sqliteConnection =>
            {
                string query = "SELECT * FROM Purchases WHERE Email = @Email";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConnection);
                command.Parameters.AddWithValue("@Email", email);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable purchasesTable = new DataTable();
                adapter.Fill(purchasesTable);
                dataGridPurchases.ItemsSource = purchasesTable.DefaultView;
            });
        }

        private void AddOrUpdateCustomer(bool isUpdate, Customer customer)
        {
            ExecuteSql(sqlConnection =>
            {
                string query = isUpdate
                    ? "UPDATE Customers SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, PhoneNumber = @PhoneNumber, Email = @Email WHERE ID = @ID"
                    : "INSERT INTO Customers (LastName, FirstName, MiddleName, PhoneNumber, Email) VALUES (@LastName, @FirstName, @MiddleName, @PhoneNumber, @Email)";

                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@MiddleName", customer.MiddleName);
                command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                command.Parameters.AddWithValue("@Email", customer.Email);

                if (isUpdate)
                {
                    command.Parameters.AddWithValue("@ID", customer.ID);
                }

                command.ExecuteNonQuery();
                MessageBox.Show(isUpdate ? "Клиент обновлен." : "Клиент добавлен.");
                LoadCustomers();
            });
        }

        private void AddPurchase(string email, int productCode, string productName)
        {
            ExecuteSQLite(sqliteConnection =>
            {
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

                        transaction.Commit();
                        MessageBox.Show("Покупка добавлена.");
                        LoadPurchases(email);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка добавления покупки: " + ex.Message);
                    }
                }
            });
        }

        private void ClearData()
        {
            ExecuteSql(sqlConnection =>
            {
                SqlCommand command = new SqlCommand("DELETE FROM Customers", sqlConnection);
                command.ExecuteNonQuery();
            });

            ExecuteSQLite(sqliteConnection =>
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM Purchases", sqliteConnection);
                command.ExecuteNonQuery();
            });

            MessageBox.Show("Все данные удалены.");
            LoadCustomers();
        }

        private void DeleteCustomer(int customerId)
        {
            ExecuteSql(sqlConnection =>
            {
                SqlCommand command = new SqlCommand("DELETE FROM Customers WHERE ID = @ID", sqlConnection);
                command.Parameters.AddWithValue("@ID", customerId);
                command.ExecuteNonQuery();
            });

            MessageBox.Show("Клиент удалён.");
            LoadCustomers();
            dataGridPurchases.ItemsSource = null;
        }

        private void dataGridCustomers_Sorting(object sender, DataGridSortingEventArgs e) => LoadCustomers();

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
            dataGridPurchases.ItemsSource = null;
        }

        private void dataGridCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                LoadPurchases(selectedCustomer.Email);
            }
        }

        //private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        //{
        //    AddCustomerWindow addCustomerWindow = new AddCustomerWindow();
        //    if (addCustomerWindow.ShowDialog() == true)
        //    {
        //        AddOrUpdateCustomer(false, new Customer
        //        {
        //            LastName = addCustomerWindow.LastName,
        //            FirstName = addCustomerWindow.FirstName,
        //            MiddleName = addCustomerWindow.MiddleName,
        //            PhoneNumber = addCustomerWindow.PhoneNumber,
        //            Email = addCustomerWindow.Email
        //        });
        //    }
        //}
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow addCustomerWindow = new AddCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
                _customerRepository.AddCustomer(new Customer
                {
                    LastName = addCustomerWindow.LastName,
                    FirstName = addCustomerWindow.FirstName,
                    MiddleName = addCustomerWindow.MiddleName,
                    PhoneNumber = addCustomerWindow.PhoneNumber,
                    Email = addCustomerWindow.Email
                });
                LoadCustomers();
            }
        }

        //private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
        //    {
        //        EditCustomerWindow editCustomerWindow = new EditCustomerWindow(selectedCustomer.LastName, selectedCustomer.FirstName, selectedCustomer.MiddleName, selectedCustomer.PhoneNumber, selectedCustomer.Email);
        //        if (editCustomerWindow.ShowDialog() == true)
        //        {
        //            selectedCustomer.LastName = editCustomerWindow.LastName;
        //            selectedCustomer.FirstName = editCustomerWindow.FirstName;
        //            selectedCustomer.MiddleName = editCustomerWindow.MiddleName;
        //            selectedCustomer.PhoneNumber = editCustomerWindow.PhoneNumber;
        //            selectedCustomer.Email = editCustomerWindow.Email;

        //            AddOrUpdateCustomer(true, selectedCustomer);
        //        }
        //    }
        //}
        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                EditCustomerWindow editCustomerWindow = new EditCustomerWindow(
                    selectedCustomer.LastName, selectedCustomer.FirstName,
                    selectedCustomer.MiddleName, selectedCustomer.PhoneNumber,
                    selectedCustomer.Email);

                if (editCustomerWindow.ShowDialog() == true)
                {
                    selectedCustomer.LastName = editCustomerWindow.LastName;
                    selectedCustomer.FirstName = editCustomerWindow.FirstName;
                    selectedCustomer.MiddleName = editCustomerWindow.MiddleName;
                    selectedCustomer.PhoneNumber = editCustomerWindow.PhoneNumber;
                    selectedCustomer.Email = editCustomerWindow.Email;

                    _customerRepository.UpdateCustomer(selectedCustomer);
                    LoadCustomers();
                }
            }
        }

        private void btnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
                if (addPurchaseWindow.ShowDialog() == true)
                {
                    AddPurchase(selectedCustomer.Email, addPurchaseWindow.ProductCode, addPurchaseWindow.ProductName);
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента.");
            }
        }

        //private void btnClearData_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("Удалить все данные?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //    {
        //        ClearData();
        //    }
        //}

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить все данные?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _customerRepository.DeleteAllCustomers();
                LoadCustomers();
            }
        }


        //private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
        //    {
        //        if (MessageBox.Show("Удалить клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //        {
        //            DeleteCustomer(selectedCustomer.ID);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выберите клиента.");
        //    }
        //}

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                _customerRepository.DeleteCustomer(selectedCustomer.ID);
                LoadCustomers();
            }
            else
            {
                MessageBox.Show("Выберите клиента.");
            }
        }
    }
}
