using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Data;

namespace Api.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RolesController(ApplicationDbContext context) => _context = context;

    // GET: api/Roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles() =>
        await _context.Roles.Where(r => r.Name != "Root").OrderBy(r => r.Name).ToListAsync();

    // POST: api/Roles
    [HttpPost]
    public async Task<ActionResult<Role>> PostRole(Role role)
    {
        if (_context.Roles.Any(r => r.Name == role.Name))
            return Conflict();

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRole", new { id = role.Id }, role);
    }

    // GET: api/Roles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound();

        return role;
    }

    // PUT: api/Roles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRole(Guid id, Role role)
    {
        if (id != role.Id)
            return BadRequest();

        _context.Entry(role).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            if (exception.GetBaseException() is SqlException sqlException)
            {
                if (sqlException.Number == 2601)
                    return Conflict();
            }
            else
            {
                if (!RoleExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        return Ok(role);
    }

    // DELETE: api/Roles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        try
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            if (exception.GetBaseException() is SqlException sqlException)
                if (sqlException.Number == 547)
                    return Conflict();
        }
        catch (Exception)
        {
            throw;
        }

        return Ok();
    }

    private Boolean RoleExists(Guid id) => _context.Roles.Any(e => e.Id == id);
}