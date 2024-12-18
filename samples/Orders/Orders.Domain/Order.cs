namespace Orders.Domain;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public void AddOrderItem(OrderItem item)
    {
        Items.Add(item);
    }

    public void RemoveOrderItem(OrderItem item)
    {
        Items.Remove(item);
    }

    public decimal GetTotal()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }
}

public class OrderItem
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
