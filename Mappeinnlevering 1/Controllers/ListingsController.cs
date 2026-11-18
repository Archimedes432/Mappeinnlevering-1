using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api/listings")]
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

    [HttpGet("{id;int}")]
    public async Task<ActionResult<ListingDtos>> GetById(int id)
    {
        var listing = await _service.GetByIdAsync(id);

        if (listing == null)
            return NotFound();

        return Ok(listing);
    }
    
    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<List<ListingDtos>>> GetMyList()
    {
        var sellerId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (sellerId == null)
            return Unauthorized();

        var listings =
            await _service.GetMyListingsAsync(sellerId);

        return Ok(listings);
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
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var isAdmin =
            User.IsInRole("Admin");

        if (userId == null)
            return Unauthorized();

        try
        {
            var updated =
                await _service.UpdateAsync(
                    id,
                    dto,
                    userId,
                    isAdmin);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
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
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var isAdmin =
            User.IsInRole("Admin");

        if (userId == null)
            return Unauthorized();

        try
        {
            var deleted =
                await _service.DeleteAsync(
                    id,
                    userId,
                    isAdmin);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}