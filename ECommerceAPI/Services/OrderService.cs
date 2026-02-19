using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
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
            var order = new Order
            {
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
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };
                total += product.Price * item.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalPrice = total;
            var created = await _orderRepository.CreateAsync(order);
            return MapToDto(created);
        }

        public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            if (!Enum.TryParse<OrderStatus>(dto.Status, out var status))
                throw new ArgumentException("Geçersiz sipariş durumu!");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            // Eğer iptal ediliyorsa stokları geri ekle
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
            return updated == null ? null : MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        private static OrderDto MapToDto(Order order) => new()
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductName = oi.Product?.Name ?? string.Empty,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.Quantity * oi.UnitPrice
            }).ToList()
        };
    }
}