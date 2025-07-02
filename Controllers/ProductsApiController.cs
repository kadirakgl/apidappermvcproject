using Microsoft.AspNetCore.Mvc;
using apidappermvcproje.Data;
using apidappermvcproje.Models;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly ProductRepository _repository;
    public ProductsApiController(ProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        var id = await _repository.CreateAsync(product);
        product.Id = id;
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id) return BadRequest();
        var updated = await _repository.UpdateAsync(product);
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