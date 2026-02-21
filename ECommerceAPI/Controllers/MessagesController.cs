using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MessagesController(AppDbContext context) => _context = context;

        // Konuşma listesi
        [HttpGet("conversations")]
        public IActionResult GetConversations()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var conversations = _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .AsEnumerable()
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => {
                    var lastMessage = g.OrderByDescending(m => m.CreatedAt).First();
                    var otherUser = lastMessage.SenderId == userId ? lastMessage.Receiver : lastMessage.Sender;
                    return new
                    {
                        UserId = otherUser!.Id,
                        Username = otherUser.Username,
                        LastMessage = lastMessage.Content,
                        LastMessageAt = lastMessage.CreatedAt,
                        UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                    };
                })
                .OrderByDescending(c => c.LastMessageAt)
                .ToList();

            return Ok(conversations);
        }

        // Belirli kullanıcıyla mesajlar
        [HttpGet("{otherUserId}")]
        public async Task<IActionResult> GetMessages(int otherUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var messages = await _context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                            (m.SenderId == otherUserId && m.ReceiverId == userId))
                .Include(m => m.Sender)
                .OrderBy(m => m.CreatedAt)
                .Select(m => new {
                    m.Id,
                    m.Content,
                    m.CreatedAt,
                    m.IsRead,
                    m.SenderId,
                    SenderUsername = m.Sender!.Username,
                    IsMine = m.SenderId == userId
                })
                .ToListAsync();

            // Okundu olarak işaretle
            var unread = await _context.Messages
                .Where(m => m.SenderId == otherUserId && m.ReceiverId == userId && !m.IsRead)
                .ToListAsync();
            unread.ForEach(m => m.IsRead = true);
            await _context.SaveChangesAsync();

            return Ok(messages);
        }

        // Mesaj gönder
        [HttpPost("{receiverId}")]
        public async Task<IActionResult> Send(int receiverId, [FromBody] SendMessageDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var message = new Message
            {
                SenderId = userId,
                ReceiverId = receiverId,
                Content = dto.Content
            };

            _context.Messages.Add(message);

            // Bildirim gönder
            _context.Notifications.Add(new Notification
            {
                UserId = receiverId,
                Title = "Yeni Mesaj! 💬",
                Message = $"Yeni bir mesajınız var.",
                Type = "info"
            });

            await _context.SaveChangesAsync();
            return Ok(new { message.Id, message.Content, message.CreatedAt });
        }

        // Okunmamış mesaj sayısı
        [HttpGet("unread-count")]
        public IActionResult GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var count = _context.Messages.Count(m => m.ReceiverId == userId && !m.IsRead);
            return Ok(new { count });
        }
        [HttpDelete("{otherUserId}")]
        public async Task<IActionResult> DeleteConversation(int otherUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var messages = await _context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                            (m.SenderId == otherUserId && m.ReceiverId == userId))
                .ToListAsync();
            _context.Messages.RemoveRange(messages);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class SendMessageDto
    {
        public string Content { get; set; } = string.Empty;
    }
}