using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null);
        Task<Product> GetByIdAsync(Guid id);
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProduct(Guid id, Product product);
        Task<Product?> DeleteProduct(Guid id);
        Task<Product?> RestoreProduct(Guid id); 
        Task<IEnumerable<Product>> GetProductsByName(string name);

        Task<bool> ProductExists(Guid id);
        void saveChanges();
    }
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _products;
        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
            _products = context.products;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _products.Include(x => x.category).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null)
        {
            var query = _context.products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.products.FindAsync(id);
        }
        public async Task<Product> AddProduct(Product product)
        {
            var entry = await _products.AddAsync(product);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Product?> UpdateProduct(Guid id, Product product)
        {
            var existingProduct = await _products.FindAsync(id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.Quantity = product.Quantity;
            existingProduct.minimumQuantity = product.minimumQuantity;
            existingProduct.category = product.category;
            existingProduct.CreatedAt = product.CreatedAt;
            existingProduct.UpdatedAt = product.UpdatedAt;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
        public async Task<Product?> DeleteProduct(Guid id)
        {
            var existingProduct = await _products.FindAsync(id);
            if (existingProduct == null)
            {
                return null;
            }
            var product = await _products.FirstOrDefaultAsync(p => p.Id == id);
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
        public async void saveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public Task<bool> ProductExists(Guid id)
        {
            return _products.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            
            return await _products.Include(x => x.category).Where(p => p.Name.Contains(name) || p.Description.Contains(name)).ToListAsync();
        }
        
        public async Task<Product?> RestoreProduct(Guid id)
        {
            var product = await GetByIdAsync(id);
            product.IsDeleted = false;
            await UpdateProduct(id, product);
            return product;
        }

    }
}
