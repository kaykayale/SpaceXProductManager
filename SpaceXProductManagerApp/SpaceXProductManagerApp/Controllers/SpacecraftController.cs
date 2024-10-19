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

    // GET: api/spacecraft
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Spacecraft>>> GetSpacecrafts()
    {
        return await _context.Spacecrafts.Include(s => s.Components).ToListAsync();
    }

    // GET: api/spacecraft/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Spacecraft>> GetSpacecraft(int id)
    {
        var spacecraft = await _context.Spacecrafts.Include(s => s.Components).FirstOrDefaultAsync(s => s.Id == id);
        
        if (spacecraft == null)
        {
            return NotFound();
        }

        return spacecraft;
    }

    // POST: api/spacecraft
    [HttpPost]
    public async Task<ActionResult<Spacecraft>> CreateSpacecraft(Spacecraft spacecraft)
    {
        _context.Spacecrafts.Add(spacecraft);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSpacecraft), new { id = spacecraft.Id }, spacecraft);
    }

    // PUT: api/spacecraft/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSpacecraft(int id, Spacecraft spacecraft)
    {
        if (id != spacecraft.Id)
        {
            return BadRequest();
        }

        _context.Entry(spacecraft).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Spacecrafts.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/spacecraft/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpacecraft(int id)
    {
        var spacecraft = await _context.Spacecrafts.FindAsync(id);
        if (spacecraft == null)
        {
            return NotFound();
        }

        _context.Spacecrafts.Remove(spacecraft);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}