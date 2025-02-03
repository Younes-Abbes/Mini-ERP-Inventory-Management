using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class SuppliersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Supplier> _suppliers;
        public SuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
            _suppliers = context.suppliers;
        }
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await _suppliers.ToListAsync();
        }
        public async Task<Supplier?> GetSupplier(Guid id)
        {
            return await _suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            var entry = await _suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Supplier?> UpdateSupplier(Guid id, Supplier supplier)
        {
            var existingSupplier = await _suppliers.FindAsync(id);
            if (existingSupplier == null)
            {
                return null;
            }
            existingSupplier.CompanyName = supplier.CompanyName;
            existingSupplier.ContactEmail = supplier.ContactEmail;
            existingSupplier.ContactPhone = supplier.ContactPhone;
            existingSupplier.CompanyAddress = supplier.CompanyAddress;
            existingSupplier.Country = supplier.Country;
            existingSupplier.PostalCode = supplier.PostalCode;
            await _context.SaveChangesAsync();
            return existingSupplier;
        }
        public async Task<Supplier?> DeleteSupplier(Guid id)
        {
            var existingSupplier = await _suppliers.FindAsync(id);
            if (existingSupplier == null)
            {
                return null;
            }
            _suppliers.Remove(existingSupplier);
            await _context.SaveChangesAsync();
            return existingSupplier;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
