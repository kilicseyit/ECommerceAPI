using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}