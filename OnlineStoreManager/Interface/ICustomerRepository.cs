using System.Collections.Generic;

public interface ICustomerRepository
{
    List<Customer> GetAllCustomers();
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int id);
    void DeleteAllCustomers();
}
