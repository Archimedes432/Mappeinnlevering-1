using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api(listings")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _service;
    
    public ListingsController(IListingService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ListingDtos>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ListingDtos>> GetById(int id)
    {
        var listing = await _service.GetByIdAsync(id);

        if (listing == null)
            return NotFound();

        return Ok(listing);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ListingDtos>> Create(
        CreateListingDto dto)
    {
        var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (sellerId == null)
            return Unauthorized();

        var listing = await _service.CreateAsync(dto, sellerId);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = listing.Id }, 
            listing);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateListingDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            
            if (!updated)
                return NotFound();
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}