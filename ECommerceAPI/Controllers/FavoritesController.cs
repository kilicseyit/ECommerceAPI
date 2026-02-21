using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FavoritesController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favorites = _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product).ThenInclude(p => p!.Category)
                .Include(f => f.Product).ThenInclude(p => p!.Reviews)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new {
                    f.Id,
                    f.ProductId,
                    f.CreatedAt,
                    Product = new
                    {
                        f.Product!.Id,
                        f.Product.Name,
                        f.Product.Price,
                        f.Product.DiscountPrice,
                        f.Product.ImageUrl,
                        f.Product.Stock,
                        CategoryName = f.Product.Category!.Name,
                        AverageRating = f.Product.Reviews.Any() ? f.Product.Reviews.Average(r => r.Rating) : 0
                    }
                })
                .ToList();
            return Ok(favorites);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var existing = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (existing != null) return BadRequest("Zaten favorilerde!");

            _context.Favorites.Add(new Favorite { UserId = userId, ProductId = productId });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var favorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (favorite == null) return NotFound();

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("ids")]
        public IActionResult GetFavoriteIds()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ids = _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToList();
            return Ok(ids);
        }
    }
}