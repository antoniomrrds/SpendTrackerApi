using Microsoft.AspNetCore.Mvc;

namespace SpendTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Expense : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("test");
    }
}
