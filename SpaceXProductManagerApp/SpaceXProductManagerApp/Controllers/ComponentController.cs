using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ComponentController : ControllerBase
{
    private readonly AppDbContext _context;

    public ComponentController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/component
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Component>>> GetComponents()
    {
        return await _context.Components.ToListAsync();
    }

    // GET: api/component/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Component>> GetComponent(int id)
    {
        var component = await _context.Components.FindAsync(id);

        if (component == null)
        {
            return NotFound();
        }

        return component;
    }

    // POST: api/component
    [HttpPost]
    public async Task<ActionResult<Component>> CreateComponent(Component component)
    {
        // Get all dependencies in a single query
        var dependencies = await _context.Components
            .Where(c => component.DependentOn.Select(d => d.DependencyId).Contains(c.Id))
            .ToListAsync();

        foreach (var dependency in dependencies)
        {
            component.ComponentDependencies.Add(new ComponentDependency
            {
                Component = component,
                Dependency = dependency
            });
        }

        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComponent), new { id = component.Id }, component);
    }

    // PUT: api/component/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComponent(int id, Component component)
    {
        if (id != component.Id)
        {
            return BadRequest();
        }

        var existingComponent = await _context.Components
            .Include(c => c.ComponentDependencies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingComponent == null)
        {
            return NotFound();
        }

        // Update basic properties
        existingComponent.Name = component.Name;
        existingComponent.Cost = component.Cost;

        // Clear existing dependencies
        _context.ComponentDependencies.RemoveRange(existingComponent.ComponentDependencies);

        // Add the new dependencies
        foreach (var dependencyId in component.DependentOn.Select(d => d.DependencyId))
        {
            var dependency = await _context.Components.FindAsync(dependencyId);
            if (dependency != null)
            {
                existingComponent.ComponentDependencies.Add(new ComponentDependency
                {
                    ComponentId = existingComponent.Id,
                    DependencyId = dependencyId
                });
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Components.Any(e => e.Id == id))
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


    // DELETE: api/component/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComponent(int id)
    {
        var component = await _context.Components
            .Include(c => c.ComponentDependencies)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (component == null)
        {
            return NotFound();
        }

        // Remove related dependencies
        _context.ComponentDependencies.RemoveRange(component.ComponentDependencies);

        _context.Components.Remove(component);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}