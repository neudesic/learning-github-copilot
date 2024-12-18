using Orders.Domain;

namespace Orders.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task<Order> GetOrderByIdAsync(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task AddOrderAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        // In-memory implementation, no action needed
        return Task.CompletedTask;
    }
}
