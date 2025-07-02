using Microsoft.AspNetCore.Mvc;
using apidappermvcproje.Data;
using apidappermvcproje.Models;

[Route("api/customers")]
[ApiController]
public class CustomersApiController : ControllerBase
{
    private readonly CustomerRepository _repository;
    public CustomersApiController(CustomerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _repository.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        var id = await _repository.CreateAsync(customer);
        customer.Id = id;
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        var updated = await _repository.UpdateAsync(customer);
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