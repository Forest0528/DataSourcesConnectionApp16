using Microsoft.EntityFrameworkCore;

public class OnlineStoreContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=OnlineStoreDB;Integrated Security=True;");
        }
    }
}
