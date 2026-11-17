using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api/offers")]
public class OffersController : ControllerBase
{
    private readonly IOfferService _service;
    
    public OffersController(IOfferService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<OfferDtos>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfferDtos>> GetById(int id)
    {
        var offer = await _service.GetByIdAsync(id);

        if (offer == null)
            return NotFound();

        return Ok(offer);
    }

    [Authorize]
    [HttpPost("{listingId}")]
    public async Task<ActionResult<OfferDtos>> Create(
        int listingId,
        CreateOfferDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var offer =
            await _service.CreateAsync(
                listingId,
                userId,
                dto);
        
        return Ok(offer);
    }

    [Authorize]
    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> withdraw(int id)
    {
        var success = await _service.WithdrawAsync(id);
        
        if (!success)
            return NotFound();
        
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        var success = await _service.AcceptAsync(id);
        
        if (!success)
            return NotFound();
        
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        var success = await _service.RejectAsync(id);
        
        if (!success)
            return NotFound();
        
        return NoContent();
    }
}