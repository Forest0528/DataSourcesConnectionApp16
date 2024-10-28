using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository
{
    private readonly OnlineStoreContext _context;

    public CustomerRepository(OnlineStoreContext context) // Конструктор с параметром
    {
        _context = context;
    }

    public List<Customer> GetAllCustomers()
    {
        return _context.Customers.ToList();
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = _context.Customers.Find(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }

    public void DeleteAllCustomers()
    {
        _context.Customers.RemoveRange(_context.Customers);
        _context.SaveChanges();
    }
}
