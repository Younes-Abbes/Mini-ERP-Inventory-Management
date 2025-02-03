using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class CustomersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Customer> _customers;
        public CustomersRepository(ApplicationDbContext context)
        {
            _context = context;
            _customers = context.customers;
        }
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _customers.Include(c => c.Orders).ThenInclude(o => o.OrderItems).ToListAsync();
        }
        public async Task<Customer?> GetCustomer(Guid id)
        {
            return await _customers.Include(c => c.Orders).ThenInclude(o => o.OrderItems).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            var entry = await _customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Customer?> UpdateCustomer(Guid id, Customer customer)
        {
            var existingCustomer = await _customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return null;
            }
            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;
            existingCustomer.Country = customer.Country;
            existingCustomer.PostalCode = customer.PostalCode;
            await _context.SaveChangesAsync();
            return existingCustomer;
        }
        public async Task<Customer?> DeleteCustomer(Guid id)
        {
            var existingCustomer = await _customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return null;
            }
            _customers.Remove(existingCustomer);
            await _context.SaveChangesAsync();
            return existingCustomer;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
