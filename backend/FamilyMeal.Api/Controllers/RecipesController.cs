using Microsoft.AspNetCore.Mvc;
using FamilyMeal.Api.Services;
using FamilyMeal.Api.Models;

namespace FamilyMeal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly RecipeService _service;

    public RecipesController(RecipeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<RecipeDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}/detail")]
    public async Task<ActionResult<RecipeDetailDto>> GetDetail(int id)
    {
        var result = await _service.GetDetailAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> Create([FromBody] CreateRecipeFullDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateRecipeDto dto)
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
