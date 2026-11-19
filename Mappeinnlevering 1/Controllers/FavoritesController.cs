using System.Security.Claims;
using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api/favorites")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favorites;

    public FavoritesController(IFavoriteService favorites)
    {
        _favorites = favorites;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    }

  
    // GET /api/favorites
    [HttpGet]
    public async Task<ActionResult<List<FavoriteDto>>> GetFavorites()
    {
        List<FavoriteDto> result = await _favorites.GetFavoritesAsync(GetUserId());
        return Ok(result);
    }

 
    // POST /api/favorites/5
    [HttpPost("{listingId:int}")]
    public async Task<IActionResult> AddFavorite(int listingId)
    {
        FavoriteResult result = await _favorites.AddFavoriteAsync(GetUserId(), listingId);

        if (result == FavoriteResult.ListingNotFound)
        {
            return NotFound();      // 404
        }
        if (result == FavoriteResult.AlreadyFavorite)
        {
            return Conflict();      // 409
        }
        return StatusCode(201);     // 201 Created
    }

  
    // DELETE /api/favorites/5
    [HttpDelete("{listingId:int}")]
    public async Task<IActionResult> RemoveFavorite(int listingId)
    {
        FavoriteResult result = await _favorites.RemoveFavoriteAsync(GetUserId(), listingId);

        if (result == FavoriteResult.NotFavorite)
        {
            return NotFound();      // 404
        }
        return NoContent();         // 204
    }
}