using System;
using System.Data;
using System.Data.SqlClient;  // Для MSSQLLocalDB
using System.Data.SQLite;     // Для SQLite
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSourcesConnectionApp
{
    public partial class Form1 : Form
    {
        string sqlConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=true;";

        string sqliteConnectionString = @"Data Source=|DataDirectory|\SecondShop.sqlite;Version=3;";

        public Form1()
        {
            InitializeComponent();
        }

        private async Task ConnectToSqlAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    lblSqlStatus.Text = "Подключено к MSSQLLocalDB";
                    txtSqlConnectionString.Text = sqlConnection.ConnectionString;
                }
                catch (Exception ex)
                {
                    lblSqlStatus.Text = "Ошибка подключения";
                    MessageBox.Show("Ошибка MSSQLLocalDB: " + ex.Message);
                }
            }
        }

        private void ConnectToSQLite()
        {
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
            {
                try
                {
                    sqliteConnection.Open();
                    lblSQLiteStatus.Text = "Подключено к SQLite";
                    txtSQLiteConnectionString.Text = sqliteConnection.ConnectionString;

                    // Создание таблиц при первом запуске
                    CreateSQLiteTables(sqliteConnection);
                }
                catch (Exception ex)
                {
                    lblSQLiteStatus.Text = "Ошибка подключения";
                    MessageBox.Show("Ошибка SQLite: " + ex.Message);
                }
            }
        }

        private void CreateSQLiteTables(SQLiteConnection connection)
        {
            string createCustomersTable = @"
                CREATE TABLE IF NOT EXISTS Customers (
                    CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT,
                    PhoneNumber TEXT
                );
            ";

            using (SQLiteCommand command = new SQLiteCommand(createCustomersTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Добавьте создание других таблиц при необходимости
        }

        private async void btnConnectSql_Click(object sender, EventArgs e)
        {
            await ConnectToSqlAsync();
        }

        private void btnConnectSQLite_Click(object sender, EventArgs e)
        {
            ConnectToSQLite();
        }
    }
}
