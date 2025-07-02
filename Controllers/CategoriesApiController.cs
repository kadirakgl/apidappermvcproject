using Microsoft.AspNetCore.Mvc;
using apidappermvcproje.Data;
using apidappermvcproje.Models;

[Route("api/categories")]
[ApiController]
public class CategoriesApiController : ControllerBase
{
    private readonly CategoryRepository _repository;
    public CategoriesApiController(CategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _repository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        var id = await _repository.CreateAsync(category);
        category.Id = id;
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (id != category.Id) return BadRequest();
        var updated = await _repository.UpdateAsync(category);
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