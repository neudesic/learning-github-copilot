using Microsoft.AspNetCore.Mvc;
using Orders.Domain;

namespace Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderRepository orderRepository, ILogger<OrderController> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }
}
