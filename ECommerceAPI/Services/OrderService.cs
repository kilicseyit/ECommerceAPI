using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, AppDbContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            try
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    CustomerName = dto.CustomerName,
                    CustomerEmail = dto.CustomerEmail,
                    OrderItems = new List<OrderItem>()
                };

                decimal total = 0;
                foreach (var item in dto.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null) throw new KeyNotFoundException($"Ürün bulunamadı: {item.ProductId}");
                    if (product.Stock < item.Quantity) throw new ArgumentException($"{product.Name} için yeterli stok yok!");

                    product.Stock -= item.Quantity;
                    var unitPrice = product.DiscountPrice ?? product.Price;
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    };
                    total += unitPrice * item.Quantity;
                    order.OrderItems.Add(orderItem);
                }

                order.TotalPrice = total;
                var created = await _orderRepository.CreateAsync(order);

                // Bildirimler
                _context.Notifications.Add(new Notification
                {
                    UserId = order.UserId,
                    Title = "Siparişiniz Alındı! 🎉",
                    Message = $"#{created.Id} numaralı siparişiniz oluşturuldu. Toplam: {order.TotalPrice} ₺",
                    Type = "success"
                });

                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        _context.Notifications.Add(new Notification
                        {
                            UserId = product.UserId,
                            Title = "Ürününüz Satıldı! 🎉",
                            Message = $"{product.Name} ürününden {item.Quantity} adet satıldı.",
                            Type = "success"
                        });

                        if (product.Stock <= 5)
                        {
                            _context.Notifications.Add(new Notification
                            {
                                UserId = product.UserId,
                                Title = "Stok Azalıyor! ⚠️",
                                Message = $"{product.Name} ürününde sadece {product.Stock} adet kaldı.",
                                Type = "warning"
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return MapToDto(created);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            if (!Enum.TryParse<OrderStatus>(dto.Status, out var status))
                throw new ArgumentException("Geçersiz sipariş durumu!");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            if (status == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        await _productRepository.UpdateAsync(product.Id, product);
                    }
                }
            }

            var updated = await _orderRepository.UpdateStatusAsync(id, status);

            // Durum değişince bildirim gönder
            if (updated != null)
            {
                _context.Notifications.Add(new Notification
                {
                    UserId = order.UserId,
                    Title = "Sipariş Durumu Güncellendi 📦",
                    Message = $"#{id} numaralı siparişinizin durumu: {dto.Status}",
                    Type = "info"
                });
                await _context.SaveChangesAsync();
            }

            return updated == null ? null : MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        private static OrderDto MapToDto(Order order) => new()
        {
            Id = order.Id,
            UserId = order.UserId,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? string.Empty,
                ProductImage = oi.Product?.ImageUrl,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.Quantity * oi.UnitPrice
            }).ToList()
        };
    }
}