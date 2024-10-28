using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace OnlineStoreManager
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<OnlineStoreContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настройка зависимостей
            var options = new DbContextOptionsBuilder<OnlineStoreContext>()
                .UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=True;")
                .Options;

            var context = new OnlineStoreContext(options);
            var customerRepository = new CustomerRepository(context);

            // Создаем окно вручную
            var mainWindow = new MainWindow(customerRepository);
            mainWindow.Show();
        }
    }
}
