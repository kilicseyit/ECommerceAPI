using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Reviews)
        .Include(p => p.User)
        .Include(p => p.OrderItems)
        .Include(p => p.Favorites)
        .Where(p => p.IsActive)
        .ToListAsync();

        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Reviews)
        .Include(p => p.User)
        .Include(p => p.OrderItems)
        .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.DiscountPrice = product.DiscountPrice;
            existing.Stock = product.Stock;
            existing.CategoryId = product.CategoryId;

            // Sadece yeni görsel geldiyse güncelle
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                existing.ImageUrl = product.ImageUrl;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            product.IsActive = false; // Soft delete!
            await _context.SaveChangesAsync();
            return true;
        }
    }
}