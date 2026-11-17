using Microsoft.AspNetCore.Mvc;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Message = "Bruktbørsen API",
            Status = "Running"
        });
    }
}