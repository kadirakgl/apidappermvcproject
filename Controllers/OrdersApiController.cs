using Microsoft.AspNetCore.Mvc;
using apidappermvcproje.Data;
using apidappermvcproje.Models;

[Route("api/orders")]
[ApiController]
public class OrdersApiController : ControllerBase
{
    private readonly OrderRepository _repository;
    public OrdersApiController(OrderRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _repository.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        var id = await _repository.CreateAsync(order);
        order.Id = id;
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        if (id != order.Id) return BadRequest();
        var updated = await _repository.UpdateAsync(order);
        if (updated == 0) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (deleted == 0) return NotFound();
        return NoContent();
    }
} 