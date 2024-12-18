namespace eShop.Basket.API.Model;

public class CustomerBasket
{
    /// <summary>
    /// Gets or sets the buyer ID.
    /// </summary>
    public string BuyerId { get; set; }

    /// <summary>
    /// Gets or sets the list of basket items.
    /// </summary>
    public List<BasketItem> Items { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerBasket"/> class.
    /// </summary>
    public CustomerBasket() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerBasket"/> class with a specified customer ID.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
    }
}
