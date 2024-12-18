using Microsoft.Extensions.Logging;

namespace Orders.Domain;

public class OrderService
{
    private readonly ILogger<Order> _logger;

    public OrderService(ILogger<Order> logger)
    {
        _logger = logger;
    }

    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public static Order CreateOrder(ILogger<Order> logger, string customerName, List<OrderItem> items)
    {
        var order = new Order()
        {
            CustomerName = customerName,
            OrderDate = DateTime.UtcNow,
            Items = items
        };
        logger.LogInformation("Order created for customer: {CustomerName}", customerName);
        return order;
    }

    public void AddOrderItem(OrderItem item)
    {
        Items.Add(item);
        _logger.LogInformation("Order item added: {ProductName}", item.ProductName);
    }

    public decimal GetTotal()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }
}


public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(int id);
    Task AddOrderAsync(Order order);
    Task SaveChangesAsync();
}
