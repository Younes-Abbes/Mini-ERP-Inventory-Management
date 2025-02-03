﻿using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class ProductsRepository
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
        public async Task<Product?> GetProduct(Guid id)
        {
            return await _products.Include(x => x.category).FirstOrDefaultAsync(p => p.Id == id);
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
            _products.Remove(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;
        }
        public async void saveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
