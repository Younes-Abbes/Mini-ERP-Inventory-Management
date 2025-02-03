using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public interface IInventoryTransactionsRepository
    {
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactions();
        Task<InventoryTransaction?> GetInventoryTransaction(Guid id);
        Task<InventoryTransaction> AddInventoryTransaction(InventoryTransaction inventoryTransaction);
        Task<InventoryTransaction?> UpdateInventoryTransaction(Guid id, InventoryTransaction inventoryTransaction);
        Task<InventoryTransaction?> DeleteInventoryTransaction(Guid id);

        Task<bool> InventoryTransactionExists(Guid id);
        void SaveChanges();
    }
    public class InventoryTransactionsRepository : IInventoryTransactionsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<InventoryTransaction> _inventoryTransactions;
        public InventoryTransactionsRepository(ApplicationDbContext context)
        {
            _context = context;
            _inventoryTransactions = context.InventoryTransactions;
        }
        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactions()
        {
            return await _inventoryTransactions.ToListAsync();
        }
        public async Task<InventoryTransaction?> GetInventoryTransaction(Guid id)
        {
            return await _inventoryTransactions.FirstOrDefaultAsync(it => it.Id == id);
        }
        public async Task<InventoryTransaction> AddInventoryTransaction(InventoryTransaction inventoryTransaction)
        {
            var entry = await _inventoryTransactions.AddAsync(inventoryTransaction);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<InventoryTransaction?> UpdateInventoryTransaction(Guid id, InventoryTransaction inventoryTransaction)
        {
            var existingTransaction = await _inventoryTransactions.FindAsync(id);
            if (existingTransaction == null)
            {
                return null;
            }
            existingTransaction.Timestamp = inventoryTransaction.Timestamp;
            existingTransaction.TransactionType = inventoryTransaction.TransactionType;
            existingTransaction.Quantity = inventoryTransaction.Quantity;
            existingTransaction.Product = inventoryTransaction.Product;
            existingTransaction.Notes = inventoryTransaction.Notes;
            existingTransaction.ReferenceId = inventoryTransaction.ReferenceId;
            await _context.SaveChangesAsync();
            return existingTransaction;
        }
        public async Task<InventoryTransaction?> DeleteInventoryTransaction(Guid id)
        {
            var existingTransaction = await _inventoryTransactions.FindAsync(id);
            if (existingTransaction == null)
            {
                return null;
            }
            _inventoryTransactions.Remove(existingTransaction);
            await _context.SaveChangesAsync();
            return existingTransaction;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> InventoryTransactionExists(Guid id)
        {
            return await _inventoryTransactions.AnyAsync(it => it.Id == id);
        }
    }
}

