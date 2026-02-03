using Microsoft.AspNetCore.Mvc;
using FamilyMeal.Api.Services;
using FamilyMeal.Api.Models;

namespace FamilyMeal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IngredientService _service;

    public IngredientsController(IngredientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredientDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientDto>> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDto>> Create([FromBody] CreateIngredientDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateIngredientDto dto)
    {
        if (!await _service.UpdateAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
