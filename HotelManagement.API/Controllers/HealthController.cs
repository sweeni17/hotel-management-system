using HotelManagement.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HotelDbContext _dbContext;

    public HealthController(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabaseAsync(CancellationToken cancellationToken)
    {
        var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

        if (!canConnect)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "Unhealthy",
                database = "hotel",
                message = "API could not connect to SQL Server."
            });
        }

        return Ok(new
        {
            status = "Healthy",
            database = "hotel",
            provider = _dbContext.Database.ProviderName
        });
    }
}
