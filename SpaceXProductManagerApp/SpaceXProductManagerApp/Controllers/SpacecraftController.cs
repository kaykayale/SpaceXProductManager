using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SpacecraftController : ControllerBase
{
    private readonly AppDbContext _context;

    public SpacecraftController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Spacecraft>>> GetSpacecrafts()
    {
        return await _context.Spacecrafts.Include(s => s.Components).ToListAsync();
    }
}