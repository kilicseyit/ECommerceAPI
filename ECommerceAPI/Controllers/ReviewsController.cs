using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/reviews")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(int productId)
        {
            var reviews = _context.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    Username = r.User!.Username,
                    IsAdmin = r.User.Role == "Admin"
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return Ok(reviews);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPending(int productId)
        {
            var reviews = _context.Reviews
                .Where(r => r.ProductId == productId && !r.IsApproved)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    Username = r.User!.Username
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return Ok(reviews);
        }
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int productId, int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            review.IsApproved = true;
            await _context.SaveChangesAsync();

            // Bunu ekle
            _context.Notifications.Add(new Notification
            {
                UserId = review.UserId,
                Title = "Yorumunuz Yayınlandı! ✅",
                Message = "Ürüne yazdığınız yorum onaylanarak yayınlandı.",
                Type = "success"
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int productId, [FromBody] CreateReviewDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                IsApproved = role == "Admin"
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Admin yorum yaptıysa ürün sahibine bildirim gönder
            if (role == "Admin")
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    _context.Notifications.Add(new Notification
                    {
                        UserId = product.UserId,
                        Title = "Ürününüze Yorum Yapıldı! 💬",
                        Message = $"{product.Name} ürününüze admin tarafından yorum yapıldı.",
                        Type = "info"
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(review);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int productId, int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CreateReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}