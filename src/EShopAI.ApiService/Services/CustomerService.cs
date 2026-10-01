using EShopAI.ApiService.Models;

namespace EShopAI.ApiService.Services;

public class CustomerService
{
    private readonly List<Customer> _customers = [];
    private readonly Lock _lock = new();

    public IReadOnlyList<Customer> GetAll()
    {
        lock (_lock)
        {
            return [.. _customers];
        }
    }

    public Customer? GetById(Guid id)
    {
        lock (_lock)
        {
            return _customers.FirstOrDefault(customer => customer.Id == id);
        }
    }

    public Customer Create(Customer customer)
    {
        lock (_lock)
        {
            customer.Id = Guid.NewGuid();
            _customers.Add(customer);
            return customer;
        }
    }

    public bool Update(Guid id, Customer updatedCustomer)
    {
        lock (_lock)
        {
            var existingCustomer = _customers.FirstOrDefault(customer => customer.Id == id);
            if (existingCustomer is null)
            {
                return false;
            }

            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;
            existingCustomer.Address = updatedCustomer.Address ?? new();
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            return _customers.RemoveAll(customer => customer.Id == id) > 0;
        }
    }
}
