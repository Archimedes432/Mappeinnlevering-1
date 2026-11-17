using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDtos>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDtos>> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);
        
        if (category == null)
            return NotFound();
        
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDtos>> Create(
        CreateCategoryDto dto)
    {
        var category = await _service.CreateAsync(dto);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            
            if (!deleted)
                return NotFound();
            
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return Conflict(
                "Category contains listings and cannot be deleted.");
        }
    }
}