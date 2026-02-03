using Microsoft.AspNetCore.Mvc;
using FamilyMeal.Api.Services;
using FamilyMeal.Api.Models;

namespace FamilyMeal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _service;

    public ReservationsController(ReservationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> Get(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("shopping-list")]
    public async Task<ActionResult<ShoppingListDto>> GetShoppingList([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate == default || endDate == default)
        {
            startDate = DateTime.Today;
            endDate = DateTime.Today.AddDays(7);
        }
        return Ok(await _service.GetShoppingListAsync(startDate, endDate));
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
